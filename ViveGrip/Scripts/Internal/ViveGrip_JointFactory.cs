using UnityEngine;
using System.Collections;

public static class ViveGrip_JointFactory {
  public static ConfigurableJoint JointToConnect(GameObject jointObject, Rigidbody desiredObject, Vector3 offset, Quaternion desiredRotation) {
    bool applyGripRotation = desiredObject.gameObject.GetComponent<ViveGrip_Grabbable>().applyGripRotation;
    ConfigurableJoint joint = jointObject.AddComponent<ConfigurableJoint>();
    ViveGrip_JointFactory.SetLinearDrive(joint, desiredObject.mass);
    ViveGrip_JointFactory.ConfigureAnchor(joint, offset, applyGripRotation);
    if (applyGripRotation) {
      ViveGrip_JointFactory.SetAngularDrive(joint, desiredObject.mass);
      ViveGrip_JointFactory.ConfigureRotation(joint, desiredObject, desiredRotation);
    }
    ViveGrip_JointFactory.Attach(joint, desiredObject);
    return joint;
  }

  private static void ConfigureAnchor(ConfigurableJoint joint, Vector3 offset, bool applyGripRotation) {
    if (applyGripRotation) { // TODO: Why is this important when we rotate? We pass in a local offset...
      joint.autoConfigureConnectedAnchor = false;
      joint.connectedAnchor = offset;
    }
    else {
      joint.anchor = offset;
    }
  }

  private static void SetLinearDrive(ConfigurableJoint joint, float mass) {
    float gripStrength = 3000f * mass;
    float gripSpeed = 10f * mass;
    float maxPower = 50f * mass;
    JointDrive jointDrive = joint.xDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    jointDrive.maximumForce = maxPower;
    joint.xDrive = jointDrive;
    jointDrive = joint.yDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    jointDrive.maximumForce = maxPower;
    joint.yDrive = jointDrive;
    jointDrive = joint.zDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    jointDrive.maximumForce = maxPower;
    joint.zDrive = jointDrive;
  }

  private static void ConfigureRotation(ConfigurableJoint joint, Rigidbody desiredObject, Quaternion desiredRotation) {
    Quaternion currentRotation = desiredObject.transform.rotation;
    joint.SetTargetRotationLocal(desiredRotation, currentRotation);
  }

  private static void SetAngularDrive(ConfigurableJoint joint, float mass) {
    float gripStrength = 3000f * mass;
    float gripSpeed = 10f * mass;
    joint.rotationDriveMode = RotationDriveMode.XYAndZ;
    JointDrive jointDrive = joint.angularYZDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    joint.angularYZDrive = jointDrive;
    jointDrive = joint.angularXDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    joint.angularXDrive = jointDrive;
  }

  private static void Attach(ConfigurableJoint joint, Rigidbody desiredObject) {
    joint.connectedBody = desiredObject;
  }
}

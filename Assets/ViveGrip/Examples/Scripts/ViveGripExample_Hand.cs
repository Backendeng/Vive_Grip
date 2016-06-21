﻿using UnityEngine;
using System.Collections;

public class ViveGripExample_Hand : MonoBehaviour {
  public Mesh rest;
  public Mesh primed;
  private float fadeSpeed = 3f;

	void Start () {}

  void ViveGripHighlightStart() {
    GetComponent<MeshFilter>().mesh = primed;
  }

  void ViveGripHighlightStop(ViveGrip_GripPoint gripPoint) {
    // We might move out of highlight range but still be holding something
    if (!gripPoint.HoldingSomething()) {
      GetComponent<MeshFilter>().mesh = rest;
    }
  }

  void ViveGripGrabStart() {
    StopCoroutine("FadeIn");
    StartCoroutine("FadeOut");
  }

  void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
    StopCoroutine("FadeOut");
    StartCoroutine("FadeIn");
    // We often are touching something when we stop grabbing
    if (!gripPoint.TouchingSomething()) {
      GetComponent<MeshFilter>().mesh = rest;
    }
  }

  IEnumerator FadeOut() {
    Color color = GetComponent<Renderer>().material.color;
    while (color.a > 0.1f) {
      color.a -= fadeSpeed * Time.deltaTime;
      GetComponent<Renderer>().material.color = color;
      yield return null;
    }
  }

  IEnumerator FadeIn() {
    Color color = GetComponent<Renderer>().material.color;
    while (color.a < 1f) {
      color.a += fadeSpeed * Time.deltaTime;
      GetComponent<Renderer>().material.color = color;
      yield return null;
    }
  }
}

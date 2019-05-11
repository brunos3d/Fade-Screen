using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenFader : MonoBehaviour {


	[Header("Control")]
	public bool doFade = true;
	[Range(0.0f, 3.0f)]
	public float time = 0.0f;
	public float speed = 1.0f;
	public float timeTarget = 1.0f;

	[Header("Graphics")]
	public int effect = 3;
	public Gradient gradient;
	public Texture texture;

	[Header("Settings")]
	public float size = 30.0f;
	[Range(10, 100)]
	public float tileSize = 20.0f;
	public Vector2 offset = Vector2.zero;

	// Update is called once per frame
	void Update() {
		if (Application.isPlaying) {
			if (doFade) {
				time = Mathf.MoveTowards(time, timeTarget, speed * Time.deltaTime);
			} else {
				time = Mathf.MoveTowards(time, 0.0f, speed * Time.deltaTime);
			}
		}
	}

	void OnGUI() {
		int width = (int)((Screen.width / tileSize) + 1);
		int height = (int)((Screen.height / tileSize) + 1);
		Color guiColor = GUI.color;

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (texture) {
					float deltaY = Mathf.InverseLerp(0.0f, height, y);
					float deltaSize = size;
					switch (effect) {
						case 0:
						deltaSize = size * time;
						GUI.color = gradient.colorKeys[0].color;
						break;

						case 1:
						deltaSize = size * time;
						GUI.color = gradient.Evaluate(deltaY);
						break;

						case 2:
						if (time >= 1 - deltaY) {
							deltaSize = size;
						} else {
							deltaSize = 0.0f;
						}
						GUI.color = gradient.colorKeys[0].color;
						break;

						case 3:
						if (time >= 1 - deltaY) {
							deltaSize = size;
						} else {
							deltaSize = 0.0f;
						}
						GUI.color = gradient.Evaluate(deltaY);
						break;

						default:
						if (time >= 1 - deltaY) {
							if ((x + y) % effect == 0) {
								deltaSize = size;
							} else {
								deltaSize = size * time;
							}
						} else {
							deltaSize = 0.0f;
						}
						GUI.color = gradient.Evaluate(deltaY);
						break;
					}
					float deltaPos = deltaSize / 2.0f;

					GUI.DrawTexture(new Rect(offset.x + x * tileSize - deltaPos, offset.y + y * tileSize - deltaPos, deltaSize, deltaSize), texture);
				}
			}
		}

		GUI.color = guiColor;
	}
}

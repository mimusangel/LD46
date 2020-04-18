using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }

	Camera cam;

	private Vector2 direction = Vector2.zero;

	Planete focus = null;
	float size = 1;
	Vector3 position;



	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(Instance);
		}
		Instance = this;
	}

	void Start()
    {
		cam = GetComponent<Camera>();
		size = cam.orthographicSize;
		position = gameObject.transform.position;
	}
	
    void Update()
    {
		if (focus == null)
		{
			cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y, 20.0f, 50.0f);
		}
		else
		{
			Vector3 pos = focus.transform.position;
			pos.z = position.z;
			gameObject.transform.position = pos;
			cam.orthographicSize = 3;
			if (Input.GetMouseButtonDown(1))
			{
				SetFocus(null);
			}
		}
    }

	public void SetFocus(Planete planete)
	{
		if (focus == planete) return;
		if (focus)
		{
			focus.HideCase();
			PanelCase.Instance.Hide();
		}
		Planete lastFocus = focus;
		focus = planete;
		if (focus != null)
		{
			focus.ShowCase();
			if (lastFocus == null)
			{
				size = cam.orthographicSize;
				position = gameObject.transform.position;
			}
			Time.timeScale = 0.0f;
		}
		else
		{
			cam.orthographicSize = size;
			gameObject.transform.position = position;
			Time.timeScale = 1.0f;
		}
	}

	public bool IsFocus(Planete planete)
	{
		return focus == planete;
	}
}

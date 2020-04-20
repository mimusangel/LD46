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
		if (Pause.Instance.IsVisible) return;
		if (focus == null)
		{
			cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y, 20.0f, 60.0f);
			if (Input.GetMouseButton(1))
			{
				float hw = Screen.width / 2.0f;
				float hh = Screen.height / 2.0f;
				Vector2 mousePos = ((Vector2)Input.mousePosition - new Vector2(hw, hh)) / new Vector2(hw, hh);
				Vector2 pos = (Vector2)transform.position + mousePos * 25.0f * Time.deltaTime;
				pos.x = Mathf.Clamp(pos.x, -90, 90);
				pos.y = Mathf.Clamp(pos.y, -50, 50);
				transform.position = new Vector3(pos.x, pos.y, transform.position.z);
			}
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
			foreach (Planete p in Planete.Planetes)
			{
				if (p != focus)
				{
					p.gameObject.SetActive(false);
				}
			}
			if (focus.isMoon)
			{
				focus.infoGold.gameObject.SetActive(true);
			}
			focus.UpdateGold();
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
			foreach (Planete p in Planete.Planetes)
			{
				p.gameObject.SetActive(true);
			}
			if (lastFocus.isMoon)
			{
				lastFocus.infoGold.gameObject.SetActive(false);
			}
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

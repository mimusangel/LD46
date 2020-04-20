using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	public Transform targetToRotate = null;
	public Vector2 worldPointToRotate = Vector2.zero;

	public Vector2 rotate = Vector2.one;

	bool hasTarget = false;

	float mDistance;
	Vector2 mDir;

	public float rotateSpeed = 5.0f;

    void Start()
    {
		if (targetToRotate != null)
		{
			Vector2 dir = new Vector2(
				Random.Range(-1.0f, 1.0f),
				Random.Range(-1.0f, 1.0f)
			);
			dir.Normalize();
			CircleCollider2D circleCollider2D = targetToRotate.gameObject.GetComponent<CircleCollider2D>();
			transform.position = (Vector2)targetToRotate.position + dir * (circleCollider2D.radius + 0.5f);
			mDistance = Vector2.Distance(transform.position, targetToRotate.position);
			mDir = (Vector2)transform.position - (Vector2)targetToRotate.position;
			hasTarget = true;
		}
		else
		{
			mDistance = Vector2.Distance(transform.position, worldPointToRotate);
			mDir = (Vector2)transform.position - worldPointToRotate;
		}
		mDir.Normalize();
	}
	
	public void ET_TP()
	{
		if (hasTarget)
		{
			targetToRotate = null;
			hasTarget = false;
			GetComponent<Planete>()?.NoMoon();
		}
		Vector3 nPos = new Vector3(
			Random.Range(-50.0f, 50.0f),
			Random.Range(-50.0f, 50.0f),
			0.0f
		);
		mDistance = Vector2.Distance(nPos, worldPointToRotate);
		mDir = (Vector2)nPos - worldPointToRotate;
		transform.position = nPos;
	}

	void RotateDir(float deg)
	{
		float sin = Mathf.Sin(deg * Mathf.Deg2Rad);
		float cos = Mathf.Cos(deg * Mathf.Deg2Rad);

		float x = mDir.x;
		float y = mDir.y;
		mDir.x = (cos * x) - (sin * y);
		mDir.y = (sin * x) + (cos * y);
	}

    void Update()
    {
		if (hasTarget && targetToRotate == null)
		{
			mDistance = Vector2.Distance(transform.position, worldPointToRotate);
			mDir = (Vector2)transform.position - worldPointToRotate;
			mDir.Normalize();

			hasTarget = false;
			GetComponent<Planete>()?.NoMoon();
		}
		RotateDir((rotateSpeed / mDistance) * Time.deltaTime);
		if (targetToRotate != null)
			transform.position = (Vector2)targetToRotate.position + mDir * rotate * mDistance;
		else
			transform.position = worldPointToRotate + mDir * rotate * mDistance;
	}
}

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
		}
		RotateDir((rotateSpeed / mDistance) * Time.deltaTime);
		if (targetToRotate != null)
			transform.position = (Vector2)targetToRotate.position + mDir * rotate * mDistance;
		else
			transform.position = worldPointToRotate + mDir * rotate * mDistance;
	}
}

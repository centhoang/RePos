using UnityEngine;

public class PointInTime {

	public Vector3 position;
	public Sprite sprite;
	public Vector3 scale;

	public PointInTime (Vector3 _position, Sprite _sprite, Vector3 _scale)
	{
		position = _position;
		sprite = _sprite;
		scale = _scale;
	}
}
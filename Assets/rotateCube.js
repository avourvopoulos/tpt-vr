var cam:Camera;

function Update()
{
    var x:float = Input.mousePosition.x - cam.WorldToScreenPoint(transform.position).x;
    var y:float = Input.mousePosition.y - cam.WorldToScreenPoint(transform.position).y;

    var zRotation:float = Mathf.Rad2Deg * Mathf.Atan2(y, x);

    transform.eulerAngles = Vector3(0, 0, zRotation);
}
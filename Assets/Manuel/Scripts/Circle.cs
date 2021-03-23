using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle {
    public float xCoordinate;
    public float yCoordinate;
    public float radius;

    public Circle(float x, float y, float r) {
        xCoordinate = x;
        yCoordinate = y;
        radius = r/2;
    }

    public Circle(Vector2 coordinates, float r) {
        xCoordinate = coordinates.x;
        yCoordinate = coordinates.y;
        radius = r / 2;
    }

    public bool Collide(Vector2 pointpeuntpount) {
        float distance = (((xCoordinate - pointpeuntpount.x) * (xCoordinate - pointpeuntpount.x)) + ((yCoordinate - pointpeuntpount.y) * (yCoordinate - pointpeuntpount.y)));
        if (distance <= (radius * radius)) {
            return true;
        }
        return false;
    }

    public bool Collide(Circle otherCircle) {
        float distance = (((xCoordinate - otherCircle.xCoordinate) * (xCoordinate - otherCircle.xCoordinate)) + ((yCoordinate - otherCircle.yCoordinate) * (yCoordinate - otherCircle.yCoordinate)));
        return (distance <= ((radius + otherCircle.radius) * (radius + otherCircle.radius)));

    }

    public Vector2 GetRandomCoordinate() {
        float decree = Random.Range(0f, 359f) * Mathf.Deg2Rad;
        float distance = Random.Range(0f, radius);
        return new Vector2(Mathf.Cos(decree) * distance + xCoordinate, Mathf.Sin(decree) * distance + yCoordinate);
    }

    public void SetPosition(Vector2 pos) {
        SetPosition(pos.x, pos.y);
    }

    public void SetPosition(float x, float y) {
        xCoordinate = x;
        yCoordinate = y;
    }
}

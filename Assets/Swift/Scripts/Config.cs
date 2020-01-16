using UnityEngine;

[System.Serializable]
public class Config
{
    [System.Serializable]
    public class Element
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;

        public Element (string name, Vector3 position, Quaternion rotation)
        {
            this.name = name;
            this.position = position;
            this.rotation = rotation;
        }
    }

    public Element[] elements;
}

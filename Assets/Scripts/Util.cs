using UnityEngine;

//have to name this Util because utility is a unity script already
public class Util  {

	public static void setLayerRecursively(GameObject obj, int newLayer)
    {
        if(obj == null)
        {
            return;
        }
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if(child == null)
            {
                continue;
            }
            setLayerRecursively(child.gameObject, newLayer);
        }
    }
}

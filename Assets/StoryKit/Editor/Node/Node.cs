using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XFramewrok.StoryKit
{
    public class Node : VisualElement
    {
        private bool isMove;
        // 用UXML
        public class Fatory : UxmlFactory<Node> { }

        public Node()
        {
            Image image1 = new Image()
            {
                image = AssetDatabase.LoadAssetAtPath<Texture>("Assets/StoryKit/Res/basic.png"),
                tintColor = Color.white,

                transform =
                {
                    position = new Vector3(0,15,0),
                }
            };
            image1.AddToClassList("connectPoint");

            Image image2 = new Image()
            {
                image = AssetDatabase.LoadAssetAtPath<Texture>("Assets/StoryKit/Res/basic.png"),
                tintColor = Color.red,

                transform =
                {
                    position = new Vector3(0,0,0),
                }
            };
            image2.AddToClassList("mainBody");

            image2.RegisterCallback<MouseMoveEvent>((a) =>
            {
                Debug.Log("MouseMove");
                if (isMove)
                    this.transform.position += (Vector3)a.mouseDelta;
            });
            image2.RegisterCallback<PointerDownEvent>((a) =>
            {
                Debug.Log("PointerDownEvent");
                this.BringToFront();
                isMove = true;
            });
            image2.RegisterCallback<PointerUpEvent>((a) =>
            {
                Debug.Log("PointerUpEvent");

                isMove = false;
            });
            image2.RegisterCallback<PointerEnterEvent>((a) =>
            {
                Debug.Log("PointerEnterEvent");

                isMove = false;
            });

            Image image3 = new Image()
            {
                image = AssetDatabase.LoadAssetAtPath<Texture>("Assets/StoryKit/Res/basic.png"),
                tintColor = Color.white,

                transform =
                {
                    position = new Vector3(0,15,0),
                }
            };
            image3.AddToClassList("connectPoint");

            Add(image1);
            Add(image2);
            Add(image3);
        }
    }
}
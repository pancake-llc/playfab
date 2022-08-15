using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pancake.GameService
{
    public class ValueExpression : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start() { GetComponent<ButtonLeaderboard>().valueExpression += () => UnityEngine.Random.Range(1, 100); }
    }
}
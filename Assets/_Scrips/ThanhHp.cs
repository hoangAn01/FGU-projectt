//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThanhHp : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Image _thanhmau;
    public void capNhatHp(float _hp, float _maxHp)
    {
        if (_thanhmau != null)
        {
            _thanhmau.fillAmount = _hp / _maxHp;
        }
    }
}

using Project.Enums;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Views
{
    public class TabBarButtonView : MonoBehaviour
    {
        public Observable<TabBarType> ButtonClicked => _button.OnClickAsObservable().Select(_ => _type);
        
        public TabBarType Type => _type;

        [SerializeField] private TabBarType _type;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GameObject _line;

        public void SetTextColor(Color color)
        {
            _text.color = color;
        }

        public void SetActiveLine(bool active)
        {
            _line.SetActive(active);
        }
    }
}
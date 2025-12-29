using Codice.Client.BaseCommands;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MicheliniDev.Utils.ScriptableVariables
{
   public class UIVariableBinder : MonoBehaviour
   {
       [Tooltip("Select the variable type to expose relevant binding options.")]
        public VariableType bindType;

        public FloatVariable floatSource;
        public IntVariable intSource;
        public StringVariable stringSource;
        public BoolVariable boolSource;

        public Slider targetSlider;
        public Image targetImage;
        public TextMeshProUGUI targetText; 
        public Toggle targetToggle;
        public GameObject targetGameObject;

        private void OnEnable()
        {
            UpdateBinding();
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (floatSource) floatSource.OnValueChanged += OnFloatChanged;
            if (intSource) intSource.OnValueChanged += OnIntChanged;
            if (stringSource) stringSource.OnValueChanged += OnStringChanged;
            if (boolSource) boolSource.OnValueChanged += OnBoolChanged;
        }

        private void Unsubscribe()
        {
            if (floatSource) floatSource.OnValueChanged -= OnFloatChanged;
            if (intSource) intSource.OnValueChanged -= OnIntChanged;
            if (stringSource) stringSource.OnValueChanged -= OnStringChanged;
            if (boolSource) boolSource.OnValueChanged -= OnBoolChanged;
        }

        private void OnFloatChanged(float val) => UpdateBinding();
        private void OnIntChanged(int val) => UpdateBinding();
        private void OnStringChanged(string val) => UpdateBinding();
        private void OnBoolChanged(bool val) => UpdateBinding();

        public void UpdateBinding()
        {
            switch (bindType)
            {
                case VariableType.Float:
                    if (floatSource == null) return;
                    if (targetSlider) targetSlider.value = floatSource.Value;
                    if (targetImage) targetImage.fillAmount = floatSource.Value;
                    if (targetText) targetText.SetText(floatSource.Value.ToString());
                    break;

                case VariableType.Int:
                    if (intSource == null) return;
                    if (targetText) targetText.SetText(intSource.Value.ToString());
                    break;

                case VariableType.String:
                    if (stringSource == null) return;
                    if (targetText) targetText.SetText(stringSource.Value.ToString());
                    break;

                case VariableType.Bool:
                    if (boolSource == null) return;
                    if (targetToggle) targetToggle.isOn = boolSource.Value;
                    if (targetGameObject && targetGameObject.activeSelf != boolSource.Value)
                        targetGameObject.SetActive(boolSource.Value);
                    break;
            }
        }
    }
}
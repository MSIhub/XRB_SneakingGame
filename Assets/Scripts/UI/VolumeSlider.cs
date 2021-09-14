using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private string  _mixerVolumeExposedParam;
    
        // Start is called before the first frame update
        private void Start()
        {
            var slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(SliderValueChanged);

            var val = PlayerPrefs.GetFloat(_mixerVolumeExposedParam, 0.75f);//if no save data, set default value
            slider.value = val;
        }

        private void SliderValueChanged(float newValue)
        {
            _mixer.SetFloat(_mixerVolumeExposedParam, 20*Mathf.Log10(newValue));
            PlayerPrefs.SetFloat(_mixerVolumeExposedParam, newValue);
            PlayerPrefs.Save();
        }
    }
}

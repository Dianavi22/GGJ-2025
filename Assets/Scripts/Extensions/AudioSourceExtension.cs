using UnityEngine;

namespace Extensions {
    public static class AudioSourceExtension {
        public static void UpdateVolume(this AudioSource source, float value) {
            if (source.volume != value) {
                source.volume = value;
            }
        }
    }
}
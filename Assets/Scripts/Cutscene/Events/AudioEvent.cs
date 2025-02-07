using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Cutscene {

    public class AudioEvent : CutsceneEvent {

        private readonly AudioEventChannel audioEventChannel;
        private readonly Audio audio;

        public AudioEvent(AudioEventChannel eventChannel, Audio clip) {

            this.audio = clip;
            this.audioEventChannel = eventChannel;
        }

        public override async Task HandleEvent(CutsceneSystem system)
        {
            Debug.Log("yeah");
            audioEventChannel.Play(audio);
        }
    }
}
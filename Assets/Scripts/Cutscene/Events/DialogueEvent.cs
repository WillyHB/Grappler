using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Cutscene
{
    public abstract class DialogueEvent
    {      
        public abstract Task HandleEvent(Dialogue system);
    }

}
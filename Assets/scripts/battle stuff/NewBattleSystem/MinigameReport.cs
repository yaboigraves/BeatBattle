using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameReport
{
    //so a minigame report needs to be initialized when we spawn the video game, and should probably be stored in the minigame itself

    //we only really need to track the current minigames report so we should track the current on in the minigame manager


    //stuff these will need is : total notes, notes correct, and the effect of the minigame node

    int totalNotes, notesCorrect;

    public int NotesCorrect
    {
        get { return notesCorrect; }

        set
        {
            notesCorrect = value;
            BattleUIManager.current.UpdateReportText(notesCorrect);

        }
    }

    public MinigameReport(int totalNotes)
    {
        this.totalNotes = totalNotes;
        notesCorrect = 0;
    }


}
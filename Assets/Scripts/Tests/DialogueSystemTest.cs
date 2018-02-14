using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using XMLFactory;

public class DialogueTest {
   public static void Main() {
     bool DEBUG = false;
     DialogueManager sys = new DialogueManager();
     sys.LoadDialogue(0);
     Dictionary<string, string> placeHolderArt = new Dictionary<string, string>();

     placeHolderArt.Add("HeroSurprised", "'o'");
     placeHolderArt.Add("HeroBored", "'-'");
     placeHolderArt.Add("HeroContemplative", "'.'?");
     placeHolderArt.Add("HeroLetDown", ";-;");

     placeHolderArt.Add("HeroineBored", "()'-')");
     placeHolderArt.Add("HeroineSurprised", "()'O')");
     placeHolderArt.Add("HeroineContemplative", "()'.')?");
     placeHolderArt.Add("HeroinePuzzled", "~()?.?)~");

     bool gameLoop = true;
     while (gameLoop) {
         string positionPadding = "                                              ";
         int newLineCount = 30;
         for (int i = 0; i < newLineCount; i++){
             Console.Write("\n");
         }
         Console.Write(placeHolderArt[sys.currentDialogue.leftSprite]);
         Console.Write(positionPadding + placeHolderArt[sys.currentDialogue.rightSprite] + "\n\n");

         if (sys.currentDialogue.currentSpeaker == "right") {
             Console.WriteLine(positionPadding + sys.currentDialogue.name);
         } else {
             Console.WriteLine(sys.currentDialogue.name);
         }
         Console.WriteLine(sys.currentDialogue.message);
         if (sys.currentDialogue.hasChoice) {
             Console.WriteLine("");
             Console.WriteLine("Enter Your Choice: ");

            for (int i = 0; i < sys.currentDialogue.choices.Count; i++) {
                Console.WriteLine(i + "). " + sys.currentDialogue.choices[i].message);
            }

            while (true) {
                PrintDebugInfo(DEBUG, sys);
                int input = Int32.Parse(Console.ReadLine());

                if (input < sys.currentDialogue.choices.Count && input > -1) {
                    sys.ProcessDialogueCommand(sys.currentDialogue.choices[input]);
                    break;
                }
            }
         } else {
             PrintDebugInfo(DEBUG, sys);
             Console.ReadLine();
         }

         gameLoop = !sys.currentDialogue.isEnd;
         sys.AdvanceDialogue();
     }
   }

   public static void PrintDebugInfo(bool debug, DialogueManager sys){
       if (debug) {
           Console.WriteLine("current dialogue ID = {0}", sys.currentDialogue.ID);
           Console.WriteLine("hasChoice = {0}", sys.currentDialogue.hasChoice);
           Console.WriteLine("current dialogue jumpID = {0}", sys.currentDialogue.jumpID);
       }
   }
}


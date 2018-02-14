using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using System;
using System.IO;

namespace XMLFactory { 
	class XMLAssembly {

		private static Dictionary<int, string> _resourceList = new Dictionary<int, string>{
			{0, "Assets/Writings/BeginnerXML.xml"}
		};
		
		public static Dictionary<int, Dialogue> RunFactoryForConversation (int sceneNumber) {
			string resourcePath = _resourceList[sceneNumber];

			IsValidXML(resourcePath);

			return ParseXML(resourcePath);			
		}

		private static void IsValidXML (string resourcePath) {
		    if (!File.Exists(resourcePath)) {
		        throw new InvalidDialogueFormatException(string.Format("Conversation file not found {0}", resourcePath));
		    }

		    if (Path.GetExtension(resourcePath) != ".xml") {
		    	throw new InvalidDialogueFormatException(string.Format("Non XML file given to parse."));
		    }
		}

		private static Dictionary<int, Dialogue> ParseXML (string xmlPath) {
     	    Dictionary<int, Dialogue> conversation = new Dictionary<int, Dialogue>();
      	    XDocument document = XDocument.Load(xmlPath);

            foreach (var parsedDialogue in document.Root.Elements("dialogue")){
                var dialogueStruct = new Dialogue();

                dialogueStruct.ID = Int32.Parse(parsedDialogue.Element("ID").Value);
	            dialogueStruct.currentSpeaker = parsedDialogue.Element("currentSpeaker").Value;
	            dialogueStruct.name = parsedDialogue.Element("name").Value;
	            dialogueStruct.leftSprite = parsedDialogue.Element("leftSprite").Value;
	            dialogueStruct.leftSpriteFlipped = parsedDialogue.Element("leftSprite").Attribute("flip") != null;
	            dialogueStruct.rightSprite = parsedDialogue.Element("rightSprite").Value;
	            dialogueStruct.rightSpriteFlipped = parsedDialogue.Element("rightSprite").Attribute("flip") != null;
	            dialogueStruct.jumpID = Int32.Parse(parsedDialogue.Element("jumpID").Value);

	            if (dialogueStruct.jumpID == dialogueStruct.ID) {
	                Console.WriteLine(@"WARNING! Self looping dialogue detected at dialogue id {0} 
	                	in file", dialogueStruct.ID);
	            }

	            dialogueStruct.message = parsedDialogue.Element("message").Value;
	            dialogueStruct.isEnd = parsedDialogue.Element("end") != null;
	            dialogueStruct.choices = new List<Choice>();

	            foreach (var parsedChoice in parsedDialogue.Elements("choice")) {
	                Choice choice = new Choice();
	                choice.message = parsedChoice.Element("message").Value;
	                bool commandExists = parsedChoice.Element("command") != null;
	                choice.command = commandExists ? parsedChoice.Element("command").Value : null;
	                choice.pointValue = commandExists ? Int32.Parse(parsedChoice.Element("command").Attribute("points").Value) : 0;
	                choice.nextJumpID = Int32.Parse(parsedChoice.Element("jumpID").Value);
	                if (choice.nextJumpID == dialogueStruct.ID) {
	                    Console.WriteLine("WARNING! Self looping dialogue detected at dialogue id {0} in choice", dialogueStruct.ID);
	                }
	                dialogueStruct.choices.Add(choice);
	            }
	            dialogueStruct.hasChoice = dialogueStruct.choices.Count > 0;
	            conversation.Add(dialogueStruct.ID, dialogueStruct);
	        }

        	return conversation;
   		}

	}
}

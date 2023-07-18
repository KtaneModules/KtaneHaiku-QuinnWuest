using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Rnd = UnityEngine.Random;
using KModkit;

public class HaikuScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;

    public KMSelectable[] ArrowSels;
    public KMSelectable SubmitSel;
    public TextMesh ScreenText;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;

    private int _currentIx;
    private int _correctIx;
    private const int _screenCount = 10;
    private string[] _selectedPhrases;

    private static readonly string[] _haikus = new string[]
    {
        "I have Who's on First.\nThe display word is “HOLD ON”.\nI read bottom right.",
        "I have Simon Says.\nThe flashing color is blue.\nI have to press red.",
        "I have a Keypad.\nThere is a copyright sign.\nI must press that first.",
        "I have Colour Flash.\nThe last color is yellow.\nI found blue in green.",
        "I have Semaphore.\nThe flags display north and east.\nLetter flags are next.",
        "I have Listening.\nThe sound playing is a cow.\nAmpersand is first.",
        "I have a Plumbing.\nI have seven batteries.\nBlue is an input.",
        "I have Safety Safe.\nI must listen for the clicks.\nThey have all been set.",
        "I have Turn The Keys.\nOops, I solved a Semaphore.\nThis bomb will explode.",
        "Ignore that strike sound.\nSoloing Double-Oh. Whoops!\nThat was a real strike.",
        "I have 3D Maze.\nMy markings are A B C.\nCardinal is north.",
        "I have Monsplode, Fight!\nI am fighting Cutie Pie.\nI must not hurt him.",
        "I have a Shape Shift.\nThe display shows ticket flat.\nI submit point round.",
        "I have Hexamaze.\nThere's a circle at F6.\nThe pawn is yellow.",
        "I have Colored Squares.\nThere is only one blue square.\nI must press that square.",
        "I have Simon Screams.\nThree clockwise colors have flashed.\nThey are red, green, blue.",
        "I have Number Pad.\nAnswer is four four two eight.\n…I have some bad news.",
        "I have Cheap Checkout.\nToday is Sweet Saturday.\nCandies are cheaper.",
        "I have Web Design.\nThere are seven lines of code.\nI must press accept.",
        "I have QR Code.\nI must scan the QR code,\nand input the code.",
        "I have Morse-A-Maze.\nThe Morse flashes the word “RABBIT”.\nI am at top left.",
        "Monsplode Trading Cards.\nThe card has two bent corners.\nIts value is low.",
        "I have a Painting.\nI have five indicators.\nTritanopia.",
        "I have Poetry.\nThe displayed girl is Lacy.\nI must press “PATIENCE”.",
        "The code is 4, 8,\n15, 16, 23,\nand then 42.",
        "Forget Everything,\nStage 2, Yellow, valid, 6.\nStage 3, invalid.",
        "I have Tax Returns.\nTime to read many numbers.\nThis is such a pain.",
        "I have Pattern Cube.\nThe red symbol is the crown.\nIt is pointing down.",
        "Marble Tumble, now.\nRed two counter, silver stays.\nPress it on a one.",
        "I have Simon Sings.\nRed, magenta, blue, and black.\nThis will not end well.",
        "Guitar Chords: B b.\nZero, two, two, two, zero.\nThat is not B flat.",
        "Calendar is red.\nDate’s November eleventh.\nI love Guy Fawkes Night!",
        "I have Coffeebucks.\nThe customer’s name is Ben.\nMaybe it was Den.",
        "Kudosudoku.\nThere is a red snooker ball,\nAnd an up arrow.",
        "I have Simon Squawks.\nIt is making lots of noise.\nWA WA WA WA WA!",
        "I have Algebra.\nTwo x plus y equals c.\nI’ll submit 18.",
        "I have Wavetapping.\nThe color is Indigo.\nTime to draw a square.",
        "Time for Tinder now.\nJane, 29, is Aries.\nShe likes clubbing dogs.",
        "In terms of bosses,\nWe have a Forget Me Not.\nWhere’s the stage display?",
        "The module is pink.\nE three one C seven nine.\nDisplay and submit.",
        "I have Spelling Bee.\nWord is “metaphysicize.”\nI’m on two strike time.",
        "Here’s your Answering.\n“Nobody likes toasted nuts.”\nRoasted or toasted?",
        "Here’s A-C-B-F.\n“What do you mean you’re at soup?”\nI’M AT THE SOUP STORE",
        "I have your answer.\nBurgerking Call'd'police\nThat's correct. Great job!",
        "Selling twelve old phones\nfor two thousand thirty three\nPolish zloty",
        "Here is Dictation.\n“Never gonna give you up...”\nI’ve just been Rickroll’d.",
        "I have Turn the Keys,\nbut the background has turned green.\nThat’s NOT Turn the Keys?",
        "I have Game of Life.\nI have checked it twenty times.\nI hope this is right.",
        "I have Cursor Maze.\nWill this module jumpscare me?\nI have trust issues.",
        "Isocolored Squares.\nSpam the buttons to solve it.\nAvocado toast.",
        "“Here's a Hand Turkey.\nRed green magenta red--” “Stop.\nSubmit Latinas.”",
        "This is a haiku.\nHaikus don’t need to make sense.\nRefrigerator.",
        "2 plus 2 is 4\nMinus 1 that’s 3 quick maths\nHa, look at your nose",
        "To end this poem,\nI need one more magic line:\nAbracadabra!",
        "Your father and I\nare taking some time apart.\nYes, because of you.",
        "Help me, I am trapped\nIn a haiku factory.\nSave me, before they",
        "I see you driving\nRound town with the girl I love,\nAnd I’m like, Haiku.",
        "We like to party.\nWe like, we like to party.\nWe like to party.",
        "My name is really\nQuinnstopher Wuestopolis.\nI have tricked you all.",
        "What have I unleashed?\nI’ve opened Pandora’s Box.\nMay God forgive me.",
        "“Write a fake haiku.”\n“It will be easy,” they said.\nBoy how they were wrong.",
        "This is a Haiku.\nAsew54321\nAnd Quinn Wuest made this.",
        "Ferdinand Johnson.\nHe was not a real person.\nIt was all a lie.",
        "Lapis Lazuli!\nCyan! Azure! Celadon!\nI am out of blues.",
        "she strogan me off\ntill i beef! EXTREMELY LOUD\nINCORRECT BUZZER"
    };
    private static readonly string[] _fakes = new string[]
    {
        "I have Who's on First.\nThe display says “nothing.”\nTop right says “display.”",
        "I have a red button.\nThe button says “DETONATE”.\nI must not hold it.",
        "I have Anagrams.\nThe displayed word is “SECURE”.\nI must submit “RESCUE.”",
        "It’s Forget Me Now.\nThis beat is pretty sick.\nPoom. Poom. Poom. Poom. Poom.",
        "Which Cycle is it?\nPigpen, Playfair, or Affine?\nAll of them, what?",
        "I have Tic Tac Toe.\nX already won the game.\nWhat is the point?",
        "Here’s Grocery Store.\nI hate the name of this mod.\nInstead here’s Shopping Store!",
        "I have your Stopwatch.\nTwo minutes and one second.\nStarting the timer now…",
        "I have a Square Button.\nThe light is flashing orange.\nI released on prime.",
        "Time for Angry Birds.\nWe have three birds and one pig.\nThere is no yellow bird.",
        "Time for Messages.\nPhil says “Probably 7”\nRob says “8 you numpty!”",
        "Time for Photos now.\nThere’s a picture of a band.\nThey look like Bon Jovi.",
        "Time for Marble Tumble.\nRed two counter, yellow stays.\nNo, I can’t do that.",
        "I have Boolean Maze.\nThe display shows number 3.\nI am stuck. Help.",
        "haha, it’s an egg!\npres at 1! it egg time!\nwhat is it doing…",
        "Chord is Delta Mike.\nFive, seven, six, blank, blank, blank.\nThat is not D minor.",
        "Wire’s white, red, blue.\n“Consume woodworking supplies”\nSure thing, Speakingevil!",
        "The date is May fifth.\nThat’s Day of German Unity.\nUm, no, that is not.",
        "I have a Word Search.\nI can’t seem to find a word.\nYes, there are no vowels.",
        "Bravo three is blank.\nBravo four has a right arrow.\nGridlock alfa four.",
        "Red, yellow, green, blue.\nYou blocked me on Facebook.\nNext stage is purple.",
        "I have Spelling Bee.\nWord is “abecedarian.”\nI’m on three strike time.",
        "Here is your answer.\nBombaboom Cabbagepatch.\nThat solved the module.",
        "Tenpins is lily,\nCincinnati, and Greek church.\nInvert, normal, invert.",
        "This module, it says:\n“Gradually watermelon.”\nEnter Shape of You.",
        "I have pressed stumble.\nRed is flashing on the right.\nIs that the Pacer Test?",
        "Here's a Crazy Talk.\nI can't help but notice\nthat the switch looks weird.",
        "Simon Shouts. How neat!\nBut what in the heck is a\nde Bruijn torus?",
        "Shape ID, how sweet.\nI'll just enter “triangle.”\nThe enter key is stuck.",
        "You have a Fishing?\nHere's a very funny trick.\nPress “C” while selected.",
        "Macro Memory.\nThere’s ten of them on the bomb.\nPress label seventeen.",
        "This is just a Sink,\nbut the drain pipe has turned black.\nI’m sure that means nothing.",
        "Where is my love?\nFeast your eyes, then let them melt.\nYou should start running.",
        "Mr. Brendan King,\nPlease finish making Death Note!\nCheers, Apolo Simon.",
        "It's a happy day.\nPlease welcome Glubtubbis Wepple\nto the orphanage.",
        "I have Colour Flash.\nOr do I? Nobody knows.\nThere’s eleven of them.",
        "This is the right one.\nNo need to read anymore.\nPress the submit button.",
        "What is going on\nI don’t know what a haiku\nIs somebody help me",
        "For sale: baby shoes.\nThey have never been worn.\nCuz’ the baby died.",
        "Time can never mend\nCareless whispers of a good friend\nTo the heart and mind",
        "Just follow my lead.\nBe careful not to make a sound!\nWhat are you doing?!",
        "Hello sir or madam.\nYour computer has virus.\nGive me three gift cards.",
        "Stop posting Among Us!\nI'm tired of seeing it!\nDing ding ding-ding-ding!"
    };

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        SubmitSel.OnInteract += SubmitPress;
        for (int i = 0; i < ArrowSels.Length; i++)
            ArrowSels[i].OnInteract += ArrowPress(i);

        _selectedPhrases = _fakes.ToArray().Shuffle().Take(_screenCount).ToArray();
        _correctIx = Rnd.Range(0, _screenCount);
        _selectedPhrases[_correctIx] = _haikus.PickRandom();
        ScreenText.text = _selectedPhrases[_currentIx];

        Debug.LogFormat("[Haiku #{0}] These are the poems: {1}.", _moduleId, _selectedPhrases.Select(i => "“" + i + "”").Join(", "));
        Debug.LogFormat("[Haiku #{0}] This is the correct haiku: {1}.", _moduleId, "“" + _selectedPhrases[_correctIx] + "”");
        Debug.LogFormat("[Haiku #{0}] Please stop reading this.", _moduleId);
    }

    private bool SubmitPress()
    {
        SubmitSel.AddInteractionPunch(0.5f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, SubmitSel.transform);
        if (_moduleSolved)
            return false;
        if (_currentIx == _correctIx)
        {
            Debug.LogFormat("[Haiku #{0}] Correct submission. Module solved.", _moduleId);
            _moduleSolved = true;
            Module.HandlePass();
            Audio.PlaySoundAtTransform("song", transform);
            return false;
        }
        else
        {
            Debug.LogFormat("[Haiku #{0}] Incorrect submission. Strike.", _moduleId);
            Module.HandleStrike();
            return false;
        }
    }

    private KMSelectable.OnInteractHandler ArrowPress(int btn)
    {
        return delegate ()
        {
            ArrowSels[btn].AddInteractionPunch(0.5f);
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, ArrowSels[btn].transform);
            if (_moduleSolved)
                return false;
            if (btn == 0)
                _currentIx = (_currentIx + 1) % _screenCount;
            if (btn == 1)
                _currentIx = (_currentIx + (_screenCount - 1)) % _screenCount;
            ScreenText.text = _selectedPhrases[_currentIx];
            return false;
        };
    }

#pragma warning disable 0414
    private readonly string TwitchHelpMessage = "!{0} left/right/submit [Press left, right, or submit.]";
#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        var m = Regex.Match(command, @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (m.Success)
        {
            yield return null;
            SubmitSel.OnInteract();
            yield break;
        }
        m = Regex.Match(command, @"^\s*left\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (m.Success)
        {
            yield return null;
            ArrowSels[0].OnInteract();
            yield break;
        }
        m = Regex.Match(command, @"^\s*right\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (m.Success)
        {
            yield return null;
            ArrowSels[1].OnInteract();
            yield break;
        }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        while (_currentIx != _correctIx)
        {
            ArrowSels[1].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        SubmitSel.OnInteract();
        yield break;
    }
}

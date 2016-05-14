using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace com.finegamedesign.anagram
{
    public class TestSyntaxModel
    {
        private static void shuffle(ArrayList cards)
        {
            for (int i = cards.Count - 1; 1 <= i; i--)
            {
                int r = (int)Mathf.Floor((Random.value % 1.0f) * (i + 1));
                dynamic swap = cards[r];
                cards[r] = cards[i];
                cards[i] = swap;
            }
        }
        
        internal string helpState;
        internal int letterMax = 10;
        internal ArrayList inputs = new ArrayList(){
        }
        ;
        /**
         * From letter graphic.
         */
        internal float letterWidth = 42.0f;
        internal delegate /*<dynamic>*/void ActionDelegate();
        internal /*<Function>*/ActionDelegate onComplete;
        internal delegate bool IsJustPressed(string letter);
        internal string help;
        internal ArrayList outputs = new ArrayList(){
        }
        ;
        internal ArrayList completes = new ArrayList(){
        }
        ;
        internal string text;
        internal ArrayList word;
        internal float wordPosition = 0.0f;
        internal float wordPositionScaled = 0.0f;
        internal int points = 0;
        internal int score = 0;
        internal string state;
        internal Levels levels = new Levels();
        private ArrayList available;
        private Dictionary<string, dynamic> repeat = new Dictionary<string, dynamic>(){
        }
        ;
        private ArrayList selects;
        private Dictionary<string, dynamic> wordHash;
        private bool isVerbose = false;
        
        public TestSyntaxModel()
        {
            wordHash = new Dictionary<string, dynamic>(){
                {
                    "aa", true}
            }
            ;
            // trial(levels.getParams());
            trial(new Dictionary<string, dynamic>(){
                {
                    "help", "Hello world"}
            }
            );
        }
        
        internal void trial(Dictionary<string, dynamic> parameters)
        {
            wordPosition = 0.0f;
            help = "";
            wordWidthPerSecond = -0.01f;
            if (null != parameters["text"]) {
                text = (string)parameters["text"];
            }
            if (null != parameters["help"]) {
                help = (string)parameters["help"];
            }
            if (null != parameters["wordWidthPerSecond"]) {
                wordWidthPerSecond = (float)parameters["wordWidthPerSecond"];
            }
            if (null != parameters["wordPosition"]) {
                wordPosition = (float)parameters["wordPosition"];
            }
            available = text.split("");
            word = new ArrayList(available);
            if ("" == help)
            {
                shuffle(word);
                wordWidthPerSecond = // -0.05;
                // -0.02;
                // -0.01;
                // -0.005;
                -0.002f;
                // -0.001;
                float power =
                // 1.5;
                // 1.75;
                2.0f;
                int baseRate = Mathf.Max(1, letterMax - text.Count);
                wordWidthPerSecond *= Mathf.Pow(baseRate, power);
            }
            selects = new ArrayList(word);
            repeat = new Dictionary<string, dynamic>(){
            }
            ;
            if (isVerbose) Debug.Log("Model.trial: word[0]: <" + word[0] + ">");
        }
        
        private int previous = 0;
        private int now = 0;
        
        internal void updateNow(int cumulativeMilliseconds)
        {
            float deltaSeconds = (now - previous) / 1000.0f;
            update(deltaSeconds);
            previous = now;
        }
        
        internal void update(float deltaSeconds)
        {
            updatePosition(deltaSeconds);
        }
        
        internal float width = 720;
        internal float scale = 1.0f;
        private float wordWidthPerSecond;
        
        internal void scaleToScreen(float screenWidth)
        {
            scale = screenWidth / width;
        }
        
        /**
         * Test case:  2015-03 Use Mac. Rosa Zedek expects to read key to change level.
         */
        private void clampWordPosition()
        {
            float wordWidth = 160;
            float min = wordWidth - width;
            if (wordPosition <= min)
            {
                help = "GAME OVER! TO SKIP ANY WORD, PRESS THE PAGEUP KEY (MAC: FN+UP).  TO GO BACK A WORD, PRESS THE PAGEDOWN KEY (MAC: FN+DOWN).";
                helpState = "gameOver";
            }
            wordPosition = Mathf.Max(min, Mathf.Min(0, wordPosition));
        }
        
        private void updatePosition(float seconds)
        {
            wordPosition += (seconds * width * wordWidthPerSecond);
            clampWordPosition();
            wordPositionScaled = wordPosition * scale;
            if (isVerbose) Debug.Log("Model.updatePosition: " + wordPosition);
        }
        
        private float outputKnockback = 0.0f;
        
        internal bool mayKnockback()
        {
            return 0 < outputKnockback && 1 <= outputs.Count;
        }
        
        /**
         * Clamp word to appear on screen.  Test case:  2015-04-18 Complete word.  See next word slide in.
         */
        private void prepareKnockback(int length, bool complete)
        {
            float perLength =
            0.03f;
            // 0.05;
            // 0.1;
            outputKnockback = perLength * width * length;
            if (complete) {
                outputKnockback *= 3;
            }
            clampWordPosition();
        }
        
        internal bool onOutputHitsWord()
        {
            bool enabled = mayKnockback();
            if (enabled)
            {
                wordPosition += outputKnockback;
                shuffle(word);
                selects = new ArrayList(word);
                for (int i = 0; i < inputs.Count; i++)
                {
                    string letter = inputs[i];
                    int selected = selects.IndexOf(letter);
                    if (0 <= selected)
                    {
                        selects[selected] = letter.ToLower();
                    }
                }
                outputKnockback = 0;
            }
            return enabled;
        }
        
        /**
         * @param   justPressed     Filter signature justPressed(letter):Boolean.
         */
        internal ArrayList getPresses(/*<Function>*/IsJustPressed justPressed)
        {
            ArrayList presses = new ArrayList(){
            }
            ;
            Dictionary<string, dynamic> letters = new Dictionary<string, dynamic>(){
            }
            ;
            for (int i = 0; i < available.Count; i++)
            {
                string letter = available[i];
                if (letters.ContainsKey(letter))
                {
                    continue;
                }
                else
                {
                    letters[letter] = true;
                }
                if (justPressed(letter))
                {
                    presses.Add(letter);
                }
            }
            return presses;
        }
        
        /**
         * If letter not available, disable typing it.
         * @return array of word indexes.
         */
        internal ArrayList press(ArrayList presses)
        {
            Dictionary<string, dynamic> letters = new Dictionary<string, dynamic>(){
            }
            ;
            ArrayList selectsNow = new ArrayList(){
            }
            ;
            for (int i = 0; i < presses.Count; i++)
            {
                string letter = presses[i];
                if (letters.ContainsKey(letter))
                {
                    continue;
                }
                else
                {
                    letters[letter] = true;
                }
                int index = available.IndexOf(letter);
                if (0 <= index)
                {
                    available.RemoveRange(index, 1);
                    inputs.Add(letter);
                    int selected = selects.IndexOf(letter);
                    if (0 <= selected)
                    {
                        selectsNow.Add(selected);
                        selects[selected] = letter.ToLower();
                    }
                }
            }
            return selectsNow;
        }
        
        internal ArrayList backspace()
        {
            ArrayList selectsNow = new ArrayList(){
            }
            ;
            if (1 <= inputs.Count)
            {
                string letter = inputs.pop();
                available.Add(letter);
                int selected = selects.LastIndexOf(letter.ToLower());
                if (0 <= selected)
                {
                    selectsNow.Add(selected);
                    selects[selected] = letter;
                }
            }
            return selectsNow;
        }
        
        /**
         * @return animation state.
         *      "submit" or "complete":  Word shoots. Test case:  2015-04-18 Anders sees word is a weapon.
         *      "submit":  Shuffle letters.  Test case:  2015-04-18 Jennifer wants to shuffle.  Irregular arrangement of letters.  Jennifer feels uncomfortable.
         * Test case:  2015-04-19 Backspace. Deselect. Submit. Type. Select.
         */
        internal string submit()
        {
            string submission = inputs.join("");
            bool accepted = false;
            state = "wrong";
            if (1 <= submission.Count)
            {
                if (wordHash.ContainsKey(submission))
                {
                    if (repeat.ContainsKey(submission))
                    {
                        state = "repeat";
                        if (levels.index <= 50 && "" == help)
                        {
                            help = "YOU CAN ONLY ENTER EACH SHORTER WORD ONCE.";
                            helpState = "repeat";
                        }
                    }
                    else
                    {
                        if ("repeat" == helpState)
                        {
                            helpState = "";
                            help = "";
                        }
                        repeat[submission] = true;
                        accepted = true;
                        scoreUp(submission);
                        bool complete = text.Count == submission.Count;
                        prepareKnockback(submission.Count, complete);
                        if (complete)
                        {
                            completes = new ArrayList(word);
                            // trial(levels.up());
                            trial(new Dictionary<string, dynamic>(){
                            }
                            );
                            state = "complete";
                            if (null != onComplete)
                            {
                                onComplete();
                            }
                        }
                        else
                        {
                            state = "submit";
                        }
                    }
                }
                outputs = new ArrayList(inputs);
            }
            if (isVerbose) Debug.Log("Model.submit: " + submission + ". Accepted " + accepted);
            inputs.Count = 0;
            available = new ArrayList(word);
            selects = new ArrayList(word);
            return state;
        }
        
        private void scoreUp(string submission)
        {
            points = submission.Count;
            score += points;
        }
        
        internal void cheatLevelUp(int add)
        {
            score = 0;
            // trial(levels.up(add));
            trial(new Dictionary<string, dynamic>(){
            }
            );
            wordPosition = 0.0f;
        }
    }
}
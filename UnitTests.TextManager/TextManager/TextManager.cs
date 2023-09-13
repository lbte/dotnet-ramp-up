using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextManager;

/// <summary>
/// general class for manager text
/// create by Miguel Angel Teheran (@Mteheran)
/// </summary>
public class TextManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextManager" /> class.
    /// </summary>
    /// <param name="strText">Text using for changes and caculate values</param>
    public TextManager(string strText, ILogger<TextManager> logger)
    {
        this.TextOriginal = strText;
        this.TextEdited = strText;
        this.logger = logger;
    }

    /// <summary>
    /// Original text asignate in cretion object
    /// </summary>
    public string TextOriginal { get; set; }

    /// <summary>
    /// Conect Text width changes in execution
    /// </summary>
    public string TextEdited { get; set; }
    private ILogger<TextManager> logger { get; set; }

    /// <summary>
    /// Count number words in orginal text (virtual so it can be overwritten for the mocks)
    /// </summary>
    /// <returns>int with counted words</returns>
    public virtual int CountWords()
    {
        // var set number word and return calculate value
        int intCount = 0;

        // Get number words
        string[] arrayTextSplit = this.TextOriginal.Split(' ');

        foreach (var item in arrayTextSplit)
        {
            // if text is not null if word
            if (!string.IsNullOrEmpty(item))
            {
                intCount++;
            }
        }

        // show info to the console
        logger.LogInformation($"Count {intCount} for text {TextOriginal}");

        return intCount;
    }

    /// <summary>
    /// Count number letter in orginal text
    /// </summary>
    /// <returns>int with counted words</returns>
    public int CountLetters()
    {
        // var set numer word and return calculate value
        int intCount = 0;

        // get number words
        string[] arrayTextSplit = this.TextOriginal.Split(' ');

        foreach (var item in arrayTextSplit)
        {
            // if text is not null if word
            if (!string.IsNullOrEmpty(item))
            {
                intCount = intCount + item.Length;
            }
        }

        return intCount;
    }

    /// <summary>
    /// Count words in text edited 
    /// </summary>
    /// <returns>int with counted words</returns>
    public int CountWordsTextEdited()
    {
        // var set numer word and return calculate value
        int intCount = 0;

        // get number words
        string[] arrayTextSplit = this.TextEdited.Split(' ');

        foreach (var item in arrayTextSplit)
        {
            // if text is not null if word
            if (!string.IsNullOrEmpty(item))
            {
                intCount++;
            }
        }

        return intCount;
    }

    /// <summary>
    /// find number words repeated in text 
    /// </summary>
    /// <param name="strWord">word to find</param>
    /// <param name="bolUpperLowerCase">Indicates when to convert text to lowercase.</param>
    /// <returns>list with position find word</returns>
    public List<int> FindWord(string strWord, bool bolUpperLowerCase)
    {
        List<int> lstFindResult = new List<int>();
        string textToFind = this.TextOriginal;

        if (bolUpperLowerCase)
        {
            textToFind = this.TextOriginal.ToLower();
            strWord = strWord.ToLower();
        }

        int i = 0;
        while ((i = textToFind.IndexOf(strWord, i)) != -1)
        {
            lstFindResult.Add(i);

            // Increment the index.
            i++;
        }

        return lstFindResult;
    }

    /// <summary>
    /// find exact number words repeated in text 
    /// </summary>
    /// <param name="strWord">word to find</param>
    /// <param name="bolIgnoreUppercaseLowercase">Indicates when to ignore case.</param>
    /// <returns>list with position find word</returns>
    public List<Match> FindExactWord(string strWord, bool bolIgnoreUppercaseLowercase)
    {
        // Define a regular expression for repeated words.
        Regex rx = new Regex(@"\b(?<word>\w+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        List<Match> lstmatch = new List<Match>();

        // Find matches.
        MatchCollection matches = rx.Matches(this.TextOriginal);

        foreach (Match item in matches)
        {
            if (bolIgnoreUppercaseLowercase)
            {
                string strTempValue = item.Value.ToLower();
                if (strTempValue.ToLower() == strWord.ToLower())
                {
                    lstmatch.Add(item);
                }
            }
            else
            {
                if (item.Value.ToLower() == strWord.ToLower())
                {
                    lstmatch.Add(item);
                }
            }
        }

        return lstmatch;
    }

    /// <summary>
    /// find number words repeated in text 
    /// </summary>
    /// <param name="strWord">word to find</param>
    /// <param name="bolIgnoreUppercaseLowercase">Indicates when to ignore case.</param>
    /// <returns>Match property width find or null if not find</returns>
    public List<Match> FindWordRepeatedContinue(string strWord, bool bolIgnoreUppercaseLowercase)
    {
        // Define a regular expression for repeated words.
        Regex rx = new Regex(@"\b(?<word>\w+)\s+(\k<word>)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        List<Match> lstmatch = new List<Match>();

        // Find matches.
        MatchCollection matches = rx.Matches(this.TextOriginal);

        foreach (Match item in matches)
        {
            if (bolIgnoreUppercaseLowercase)
            {
                string strTempValue = item.Value.ToLower();
                if (strTempValue.ToLower().Contains(strWord.ToLower()))
                {
                    lstmatch.Add(item);
                }
            }
            else
            {
                if (item.Value.ToLower().Contains(strWord.ToLower()))
                {
                    lstmatch.Add(item);
                }
            }
        }

        return lstmatch;
    }

    /// <summary>
    /// find number words repeated continuos in text 
    /// </summary>
    /// <returns>Match property width find or null if not find</returns>
    public MatchCollection FindWordAllRepeatedContinue()
    {
        // Define a regular expression for repeated words.
        Regex rx = new Regex(@"\b(?<word>\w+)\s+(\k<word>)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Find matches.
        MatchCollection matches = rx.Matches(this.TextOriginal);

        return matches;
    }
}


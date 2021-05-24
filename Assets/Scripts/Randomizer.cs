using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomizer : IRandomizerAnswers
{
    public void RandomAnswer(List<string> allAnswers, int indexLevel, SpawnerAnswersButton spawnerAnswersButton, Text question, out int correctAnswer)
    {
        List<string> answers = new List<string>();
        answers.AddRange(allAnswers);
        List<string> selectedAnswers = new List<string>();

        for(int i = 0; i < 3 + (3 * indexLevel); i++)
        {
            selectedAnswers.Add(answers[Random.Range(0, answers.Count)]);
            spawnerAnswersButton.Answers[i].TextWithAnswer.text = selectedAnswers[i];
            answers.Remove(selectedAnswers[i]);
        }

        correctAnswer = Random.Range(0, selectedAnswers.Count);
        question.text = "Find " + selectedAnswers[correctAnswer];
        allAnswers.Remove(selectedAnswers[correctAnswer]);
    }
}

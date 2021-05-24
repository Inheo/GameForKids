using System.Collections.Generic;
using UnityEngine.UI;

public interface IRandomizerAnswers
{
    void RandomAnswer(List<string> allAnswers, int indexLevel, SpawnerAnswersButton spawnerAnswersButton, Text question, out int correctAnswer);
}

using System.Collections.Generic;
using UnityEngine;

public class SpawnerAnswersButton : MonoBehaviour
{
    public InformationAboutTheAnswerCell prefabAnswer;
    public Transform parentAnswers;
    public List<InformationAboutTheAnswerCell> Answers;
    
    public Level Level;
    
    public void Spawn()
    {
        for(int i = Level.IndexLevel * 3; i < 3 + (3 * Level.IndexLevel); i++)
        {
            InformationAboutTheAnswerCell btn = Instantiate(prefabAnswer, parentAnswers);
            btn.name = i.ToString();
            btn.Index = i;
            btn.Button.onClick.AddListener(() => {Level.CheckResult(btn);});
            Answers.Add(btn);
        }
    }
}

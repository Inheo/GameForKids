using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class Level : MonoBehaviour
{
    [SerializeField] private SpawnerAnswersButton spawnerAnswersButton;
    [SerializeField] private Text question;
    [SerializeField] private GameObject answersPanel;
    [SerializeField] private GridLayoutGroup parentAnswersGrid;
    [SerializeField] private CanvasGroup groupAnswers;
    [SerializeField] private ParticleSystem[] starParticles;
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private CanvasGroup loadScreen;
    public float SpeedAnimationShowAnswersPanel = 0.5f;
    public Ease AnimationShowAnswersPanel;

    public static int IndexLevel {get; private set;}

    public UnityAction onWin;

    private int correctAnswer;
    
    private List<string> selectedCharacters;
    private List<string> allAnswers = new List<string>();

    private Randomizer randomizer = new Randomizer();
    private void Awake()
    {
        ResetValue();
        StartValue();
        spawnerAnswersButton.Spawn();
        ShowQuestion();
        onWin += NextLevel;
    }

    private void ShowAnswersPanel()
    {
        answersPanel.transform.DOScale(Vector3.one, SpeedAnimationShowAnswersPanel).SetEase(AnimationShowAnswersPanel);
    }

    private void ShowQuestion()
    {
        randomizer.RandomAnswer(allAnswers, IndexLevel, spawnerAnswersButton, question, out correctAnswer);
        question.DOFade(1, SpeedAnimationShowAnswersPanel).OnComplete(() => {ShowAnswersPanel();});
    }

    private void StartValue()
    {
        answersPanel.transform.localScale = Vector3.zero;
        question.color = new Color(1, 1, 1, 0) * question.color;
        parentAnswersGrid.enabled = true;
    }

    private void ResetValue()
    {
        allAnswers.Clear();
        allAnswers.AddRange(DB.SymbolsForAnswers);
        IndexLevel = 0;
    }
    private void NextLevel()
    {
        IndexLevel++;
        if(IndexLevel >= 3)
        {
            ShowBlackScreen();
        }
        else
        {
            parentAnswersGrid.enabled = true;
            spawnerAnswersButton.Spawn();
            randomizer.RandomAnswer(allAnswers, IndexLevel, spawnerAnswersButton, question, out correctAnswer);
        }
    }

    private void ShowStartParticles()
    {
        foreach(var particle in starParticles)
        {
            particle.Play();
        }
    }

    private void ShowBlackScreen()
    {
        groupAnswers.interactable = false;
        blackScreen.DOFade(1, 0.5f);
        blackScreen.blocksRaycasts = true;
    }

    private void HideBlackScreen()
    {
        groupAnswers.interactable = true;
        blackScreen.DOFade(0, 0.8f);
        blackScreen.blocksRaycasts = false;
    }
    private void HideLoadScreen()
    {
        loadScreen.DOFade(0, 0.8f).OnComplete(() => {loadScreen.gameObject.SetActive(false);});
    }
    private void RestartLevel()
    {
        ResetValue();

        DestroyAnswersCell();

        parentAnswersGrid.enabled = true;
        spawnerAnswersButton.Spawn();

        randomizer.RandomAnswer(allAnswers, IndexLevel, spawnerAnswersButton, question, out correctAnswer);
        HideLoadScreen();
        HideBlackScreen();
        
    }

    private void DestroyAnswersCell()
    {
        for(int i = 0; i < spawnerAnswersButton.Answers.Count; i++)
        {
            Destroy(spawnerAnswersButton.Answers[i].gameObject);
        }
        spawnerAnswersButton.Answers.Clear();
    }

    public void ShowLoadScreen()
    {
        loadScreen.gameObject.SetActive(true);
        loadScreen.DOFade(1, 1f).OnComplete(RestartLevel);
    }
    public void CheckResult(InformationAboutTheAnswerCell element)
    {
        if(correctAnswer == element.Index)
        {
            ShowStartParticles();
            element.transform.DOScale(Vector3.one * 1.3f, SpeedAnimationShowAnswersPanel).SetEase(AnimationShowAnswersPanel)
                                .OnComplete(() => 
                                {
                                    element.transform.localScale = Vector3.one;
                                    onWin?.Invoke();
                                });
        }
        else
        {
            parentAnswersGrid.enabled = false;
            element.transform.DOShakePosition(0.5f, 10, 90);
        }
    }
    
    private void OnDisable()
    {
        onWin -= NextLevel;
    }
}

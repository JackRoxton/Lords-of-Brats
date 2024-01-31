using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.SplashScreen;
using UnityEngine.Playables;

[System.Serializable]
public class Dialogue {
    public string key;
    public dialogueStruct[] dialogues;
    public UnityEvent callAtEnd;
}

[System.Serializable]
public struct dialogueStruct {
    [TextArea(3, 10)]
    public string sentence;
}

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    private Animator animator;
    [Header("UI Assets")]
    public GameObject MainMenu;
    public GameObject Ending;
    public GameObject DialogBox;
    public TMP_Text Wave;

    [Header("Texts")]
    public Dialogue[] dictionnary;
    public TextMeshProUGUI sentenceText;
    public Queue<dialogueStruct> sentences;

    public PlayableDirector WaveAnimation;

    UnityEvent functionToCall;
    bool antiSpeed = true;

    private void Awake() {
        Instance = this;
        animator = GetComponent<Animator>();
        DialogBox.SetActive(false);
    }
    public void Update() {
        if (Input.GetKeyDown(KeyCode.I)) UIManager.Instance.LaunchDialogue("Intro");
        if (Input.GetKeyDown(KeyCode.F)) UIManager.Instance.LaunchDialogue("Fin");
    }

    public void LaunchDialogue(string key) {
        DialogBox.SetActive(true);
        //SoundManager.Instance.Music.volume = 0.5f;
        Dialogue d = Array.Find(dictionnary, dialogue => dialogue.key == key);
        if (d == null) {
            Debug.Log("Dialogue " + key + " not found");
            return;
        }
        functionToCall = d.callAtEnd;
        sentences = new Queue<dialogueStruct>();
        foreach (dialogueStruct dialogue in d.dialogues) sentences.Enqueue(dialogue);
        animator.SetBool("isOpen", true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (!DialogBox.activeSelf || !antiSpeed) return;
        if (sentences == null) { OnEndSentences(); return; }
        if (sentences.Count == 0) { OnEndSentences(); return; }
        dialogueStruct dialogue = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogue.sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        sentenceText.text = "";
        antiSpeed = false;
        foreach (char letter in sentence.ToCharArray()) {
            sentenceText.text += letter;
            yield return new WaitForSeconds(.02f);
        }
        antiSpeed = true;
    }

    public void OnEndSentences() {
        animator.SetBool("isOpen", false);
        //SoundManager.Instance.Music.volume = 1f;
        DialogBox.SetActive(false);
        if(functionToCall != null) functionToCall.Invoke();
    }

    private IEnumerator PlayTimeline(int anim) {
        Wave.text = "Wave " + (anim + 1);
        WaveAnimation.Play();
        yield return new WaitForSeconds((float)WaveAnimation.duration);
        if (anim == 2) GameManager.Instance.Player.transform.position = Vector3.zero;
        else GameManager.Instance.State = GameState.Game;
    }
    public void PlayAnimation(int anim) {
        StopAllCoroutines();
        StartCoroutine(PlayTimeline(anim));
    }
    public void ChangeSceneState(GameObject scene) {
        scene.SetActive(!scene.activeSelf);
    }
    public void Reload() {
        SceneManager.LoadScene(0);
    }
}
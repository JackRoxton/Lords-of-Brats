using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.SplashScreen;

[System.Serializable]
public class Dialogue {
    public string key;
    public dialogueStruct[] dialogues;
}

[System.Serializable]
public struct dialogueStruct {
    [TextArea(3, 10)]
    public string sentence;
}

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    private Animator animator;

    public Queue<dialogueStruct> sentences;
    public Dialogue[] dictionnary;

    public GameObject DialogBox;
    public TextMeshProUGUI sentenceText;
    bool antiSpeed = true;

    private void Awake() {
        Instance = this;
        animator = GetComponent<Animator>();
        DialogBox.SetActive(false);
        LaunchDialogue("Intro");
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
    }
    public void Reload() {
        SceneManager.LoadScene(0);
    }
}
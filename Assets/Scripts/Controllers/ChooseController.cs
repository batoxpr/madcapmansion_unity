using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseController : MonoBehaviour
{
    public ChooseLabelController label;
    public GameController gameController;
    public TextMeshProUGUI titlePrefab; // Prefab reference for the title
    private RectTransform rectTransform;
    private Animator animator;
    private float labelHeight = -1;
    private TextMeshProUGUI titleInstance; // The actual instance of the title

    void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

  public void SetupChoose(ChooseScene scene)
{
    DestroyLabels();
    animator.SetTrigger("Show");

    // Calculate total label height for centering
    float totalLabelsHeight = scene.labels.Count * labelHeight;
    float centeredY = totalLabelsHeight / 2;

    // Position the title above the centered labels
    if (titlePrefab != null)
    {
        titleInstance = Instantiate(titlePrefab, transform);
        titleInstance.text = scene.title;

        // Anchor and position the title
        titleInstance.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        titleInstance.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        titleInstance.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        titleInstance.rectTransform.sizeDelta = new Vector2(600, 50); // Adjust width as needed
        titleInstance.rectTransform.anchoredPosition = new Vector2(0, centeredY + 60); // Adjust vertical offset above the first label

        titleInstance.enableWordWrapping = false;
        titleInstance.alignment = TextAlignmentOptions.Center;
    }

    // Position the labels centered on the screen
    for (int index = 0; index < scene.labels.Count; index++)
    {
        ChooseLabelController newLabel = Instantiate(label.gameObject, transform).GetComponent<ChooseLabelController>();

        if (labelHeight == -1)
        {
            labelHeight = newLabel.GetHeight();
        }

        // Position each label relative to the center
        float labelPositionY = centeredY - (labelHeight * index);

        newLabel.Setup(scene.labels[index], this, labelPositionY);
    }

    // Adjust the RectTransform to accommodate both title and labels
    Vector2 size = rectTransform.sizeDelta;
    size.y = totalLabelsHeight + 100; // Extra space for the title and margins
    rectTransform.sizeDelta = size;
}




    public void PerformChoose(StoryScene scene)
    {
        gameController.PlayScene(scene);
        animator.SetTrigger("Hide");
    }

    private float CalculateLabelPosition(int labelIndex, int labelCount)
    {
        float basePosition = labelHeight; // Offset due to the title
        if (labelCount % 2 == 0)
        {
            if (labelIndex < labelCount / 2)
            {
                return basePosition + labelHeight * (labelCount / 2 - labelIndex - 1) + labelHeight / 2;
            }
            else
            {
                return basePosition - (labelHeight * (labelIndex - labelCount / 2) + labelHeight / 2);
            }
        }
        else
        {
            if (labelIndex < labelCount / 2)
            {
                return basePosition + labelHeight * (labelCount / 2 - labelIndex);
            }
            else if (labelIndex > labelCount / 2)
            {
                return basePosition - (labelHeight * (labelIndex - labelCount / 2));
            }
            else
            {
                return basePosition;
            }
        }
    }

    private void DestroyLabels()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
    }
}

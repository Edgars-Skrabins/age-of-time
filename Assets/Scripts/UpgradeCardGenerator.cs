using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeCardGenerator : MonoBehaviour
{
    [SerializeField] private int m_amountOfCardsToSpawn;
    [SerializeField] private float m_generationRate;
    [SerializeField] private UpgradeCard m_upgradeCardPrefab;
    [SerializeField] private UpgradeCardSO[] m_upgradeCardSOs;

    private List<UpgradeCard> m_spawnedUpgradeCards;
    private float m_generationTimer;

    private void Update()
    {
        if (ShouldCountGenerationTimer())
        {
            CountGenerationTimer();
        }

        if (ShouldGenerateCards())
        {
            GenerateCards();
        }
    }

    private bool ShouldGenerateCards()
    {
        return m_generationTimer >= m_generationRate && m_spawnedUpgradeCards.Count < m_amountOfCardsToSpawn;
    }

    private bool ShouldCountGenerationTimer()
    {
        return m_generationTimer < m_generationRate;
    }

    private void CountGenerationTimer()
    {
        m_generationTimer += Time.deltaTime;
    }

    private void ResetGenerationTimer()
    {
        m_generationTimer = 0f;
    }

    private void HandleCardPicked()
    {
        DeleteAllSpawnedCards();
        ResetGenerationTimer();
    }

    private void DeleteAllSpawnedCards()
    {
        foreach (UpgradeCard card in m_spawnedUpgradeCards)
        {
            Destroy(card.gameObject);
        }
        m_spawnedUpgradeCards.Clear();
    }

    private void GenerateCards()
    {
        for (int i = 0; i < m_amountOfCardsToSpawn; i++)
        {
            UpgradeCardSO upgradeCardSO = m_upgradeCardSOs[Random.Range(0, m_upgradeCardSOs.Length)];
            UpgradeCard upgradeCard = Instantiate(m_upgradeCardPrefab, transform);
            m_spawnedUpgradeCards.Add(upgradeCard);
            upgradeCard.SetButtonImage(upgradeCardSO.m_sprite);
            upgradeCard.OnButtonClick += HandleCardPicked;
        }
    }
}
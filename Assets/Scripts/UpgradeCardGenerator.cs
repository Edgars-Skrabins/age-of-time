using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeCardGenerator : MonoBehaviour
{
    [SerializeField] private int m_amountOfCardsToSpawn;
    [SerializeField] private float m_generationRate;
    [SerializeField] private UpgradeCard[] m_upgradeCardPrefabs;

    private readonly List<UpgradeCard> m_spawnedUpgradeCards = new List<UpgradeCard>();
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

    private void HandleCardPicked(int _upgradeCost)
    {
        LevelManager.I.RemoveTime(_upgradeCost);
        DeleteAllSpawnedCards();
        ResetGenerationTimer();
        VoiceoverManager.I.Play("Player_Upgrade");
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
        List<UpgradeCard> m_upgradeCardPrefabsCopy = new List<UpgradeCard>(m_upgradeCardPrefabs);
        m_upgradeCardPrefabsCopy.RemoveAll(card => !card.ShouldBeAvailable());

        for (int i = 0; i < m_amountOfCardsToSpawn; i++)
        {
            UpgradeCard upgradeCardPrefab = m_upgradeCardPrefabsCopy[Random.Range(0, m_upgradeCardPrefabsCopy.Count)];
            m_upgradeCardPrefabsCopy.Remove(upgradeCardPrefab);
            UpgradeCard upgradeCard = Instantiate(upgradeCardPrefab, transform);
            m_spawnedUpgradeCards.Add(upgradeCard);
            upgradeCard.OnButtonClick += HandleCardPicked;
        }
    }
}
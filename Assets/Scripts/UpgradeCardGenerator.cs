using UnityEngine;

public class UpgradeCardGenerator : MonoBehaviour
{
    [SerializeField] private int m_amountOfCardsToSpawn;
    [SerializeField] private UpgradeCard m_upgradeCardPrefab;
    [SerializeField] private UpgradeCardSO[] m_upgradeCardSOs;
    private UpgradeCard[] m_spawnedUpgradeCards;

    private void HandleCardPicked()
    {
        DeleteAllSpawnedCards();
    }

    private void DeleteAllSpawnedCards()
    {
        foreach (UpgradeCard card in m_spawnedUpgradeCards)
        {
            Destroy(card.gameObject);
        }
    }

    private void GenerateCards()
    {
        for (int i = 0; i < m_amountOfCardsToSpawn; i++)
        {
            UpgradeCardSO upgradeCardSO = m_upgradeCardSOs[Random.Range(0, m_upgradeCardSOs.Length)];
            UpgradeCard upgradeCard = Instantiate(m_upgradeCardPrefab, transform);
            upgradeCard.SetButtonImage(upgradeCardSO.m_sprite);
            upgradeCard.OnButtonClick += HandleCardPicked;
        }
    }
}
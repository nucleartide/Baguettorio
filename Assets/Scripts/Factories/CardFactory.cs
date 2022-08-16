using UnityEngine;

/// <summary>
/// Creates cards from a Card prefab.
/// </summary>
public class CardFactory : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    CardPresenter cardPrefab;

    [SerializeField]
    [NotNull]
    GameViewPresenter gameViewPresenter;

    [SerializeField]
    [NotNull]
    CurrentlySelectedCard currentlySelectedCard;

    [SerializeField]
    [NotNull]
    PlayingFieldPresenter playingFieldPresenter;

    public CardPresenter Build(Card card) => Build(card, Vector3.zero, Quaternion.identity);
    public CardPresenter Build(Card card, Vector3 position, Quaternion rotation)
    {
        var cardPresenter = Instantiate(cardPrefab, position, rotation);
        cardPresenter.Model = card;
        cardPresenter.GameViewPresenter = gameViewPresenter;
        cardPresenter.CurrentlySelectedCard = currentlySelectedCard;
        cardPresenter.PlayingFieldPresenter = playingFieldPresenter;
        return cardPresenter;
    }
}

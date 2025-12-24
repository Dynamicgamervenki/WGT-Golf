using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomControls
{
    public class FeaturedCard : VisualElement
    {
        public new class UxmlFactory :  UxmlFactory<FeaturedCard> { }
        private ScrollView _scrollView;
        private List<Cards> _cards = new List<Cards>();
        private List<VisualElement> _bullets = new List<VisualElement>();
        private int _currentContentIndex;
        private float _switchInterval = 2f;
        private double _lastSwitchTime = 0;

        private VisualElement _bulletContainer;
        private readonly Color _activeColor = new Color(0.698f, 1f, 0.271f, 1f);
        private readonly Color _inactiveColor =  new Color(1f, 1f, 1f, 0.2f);
        private float _cardWidth = 600f;
        
        public FeaturedCard()
        {
            var featuredCardStyleSheet = Resources.Load<StyleSheet>("Styles/FeaturedCard");
            styleSheets.Add(featuredCardStyleSheet);
            
            var container = new  VisualElement { name = "CardContainer" };
            _bulletContainer = new VisualElement { name = "CardBulletContainer"};
            
            SetupCardHeader();


            _scrollView = new ScrollView(ScrollViewMode.Horizontal);
            _scrollView.name = "CardScrollView";
            _scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            _scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
            
           
            container.AddToClassList("cardContainer");
            _scrollView.AddToClassList("cardScrollView");
            _bulletContainer.AddToClassList("bulletContainer");
            
            Add(container);
            container.Add(_scrollView);
            Add(_bulletContainer);

            InitializeCards();
            schedule.Execute(AutoSwitchCard).Every(100);
        }

        private void SetupCardHeader()
        {
            var header = new VisualElement { name = "CardHeader" };
            header.AddToClassList("featuredHeader");
            
            var featured = new Label { name = "Featured" };
            featured.AddToClassList("headerText");
            featured.text = "Featured";
            
            var countDown = new Label { name = "CountDown" };
            countDown.AddToClassList("headerText");
            countDown.text = "00d 00h";
            
            Add(header);
            header.Add(featured);
            header.Add(countDown);
        }

        private void InitializeCards()
        {
            var coinChaos = new Cards("Coin Chaos","Lorem ipsum doior sit amet");
            var shakeDown = new Cards("Shake Down", "Lorem ipsum dolor sit amet");
            var winnersCircle = new Cards("Winners Circle", "Lorem ipsum dolor sit amet");
            var valhalla = new Cards("Valhalla Championship", "Lorem ipsum dolor sit amet");
            
            AddToCardsList(coinChaos);
            AddToCardsList(shakeDown);
            AddToCardsList(winnersCircle);
            AddToCardsList(valhalla);

            foreach (var card in _cards)
            {
                var newCard = new VisualElement();
                newCard.name = card.Name;
                newCard.AddToClassList("card");
                
                var texture =  Resources.Load<Texture2D>(card.Name);
                newCard.style.backgroundImage = new StyleBackground(texture);
                _scrollView.Add(newCard);

                var modeCard = new VisualElement{name = "ModeCard"};
                modeCard.AddToClassList("modeCard");
                newCard.Add(modeCard);
                
                var cardTitle = new Label("Card Title");
                var cardDescription = new Label("Card Description");
                cardTitle.AddToClassList("cardTitleText");
                cardTitle.text = card.Name;
                cardDescription.AddToClassList("cardDescriptionText");
                cardDescription.text = card.Description;
                
                
                modeCard.Add(cardTitle);
                modeCard.Add(cardDescription);
                
                
                var bullet = new VisualElement();
                _bullets.Add(bullet);
                bullet.name = "bullet";
                bullet.AddToClassList("bullet");
                bullet.style.backgroundColor = _inactiveColor;
                _bulletContainer.Add(bullet);
            }
            UpdateBullets();
            
        }

        private void AddToCardsList(Cards card)
        {
            _cards.Add(card);
        }
        
        
        private void AutoSwitchCard()
        {
            if (!(Time.realtimeSinceStartup - _lastSwitchTime >= _switchInterval)) return;
            _currentContentIndex++;
            if (_currentContentIndex >= _cards.Count)
                _currentContentIndex = 0;
                
            _scrollView.scrollOffset = new Vector2(_currentContentIndex * _cardWidth, 0);
            UpdateBullets();
                
            _lastSwitchTime = Time.realtimeSinceStartup;
        }
        
        private void UpdateBullets()
        {

            for (int i = 0; i < _bullets.Count; i++)
            {
                if (i == _currentContentIndex)
                    _bullets[i].style.backgroundColor = _activeColor;
                else
                    _bullets[i].style.backgroundColor = Color.gray;
            }
        }
    }

    public class Cards
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Cards(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
    
}









// _scrollView.RegisterCallback<WheelEvent>(evt =>
// {
//     if (evt.delta.y > 0)
//         ScrollToNextCard();
//     else
//         ScrollToPreviousCard();
//
//     evt.StopPropagation(); 
// });

#include "GameScene.h"

USING_NS_CC;

Scene* GameSence::createScene()
{
	return GameSence::create();
}

// on "init" you need to initialize your instance
bool GameSence::init()
{
	//////////////////////////////
	// 1. super init first
	if (!Scene::init())
	{
		return false;
	}

	//add touch listener
	EventListenerTouchOneByOne* listener = EventListenerTouchOneByOne::create();
	listener->setSwallowTouches(true);
	listener->onTouchBegan = CC_CALLBACK_2(GameSence::onTouchBegan, this);
	Director::getInstance()->getEventDispatcher()->addEventListenerWithSceneGraphPriority(listener, this);

	Size visibleSize = Director::getInstance()->getVisibleSize();
	Vec2 origin = Director::getInstance()->getVisibleOrigin();

	stoneLayer = Layer::create();
	stoneLayer->setPosition(Vec2(0, 0));
	this->addChild(stoneLayer);

	mouseLayer = Layer::create();
	mouseLayer->setPosition(Vec2(0, visibleSize.height / 2));
	this->addChild(mouseLayer);

	auto background = Sprite::create("level-background-0.jpg");
	background->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y));
	this->addChild(background, 0);

	auto label = Label::createWithTTF("Shoot", "fonts/Marker Felt.ttf", 50);	
	auto menuItem = MenuItemLabel::create(label, CC_CALLBACK_1(GameSence::shootMenuCallback,this));
	menuItem->setPosition(Vec2(visibleSize.width / 2 + origin.x + 330, visibleSize.height / 2 + origin.y + 180));
	auto menu = Menu::create(menuItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, 1);

	stone = Sprite::create("stone.png");
	stone->setPosition(Vec2(560,480));
	this->addChild(stone, 1);

	mouse = Sprite::createWithSpriteFrameName("gem-mouse-0.png");
	Animate* mouseAnimate = Animate::create(AnimationCache::getInstance()->getAnimation("mouseAnimation"));
	mouse->runAction(RepeatForever::create(mouseAnimate));
	mouse->setPosition(Vec2(visibleSize.width / 2 + origin.x,0));
	this->addChild(mouse, 1);

	return true;
}

bool GameSence::onTouchBegan(Touch *touch, Event *unused_event) {

	auto location = touch->getLocation();
	auto cake = Sprite::create("cheese.png");
	cake->setPosition(location);
	auto fadeOut = FadeOut::create(2.0f);
	cake->runAction(fadeOut);
	this->addChild(cake, 1);

	auto moveTo = MoveTo::create(1.0f, location);
	mouse->runAction(moveTo);

	return true;
}

void GameSence::shootMenuCallback(Ref * pSender)
{
	auto location = mouse->getPosition();
	auto diamond = Sprite::create("diamond.png");
	diamond->setPosition(location);
	this->addChild(diamond,1);

	auto stone1 = Sprite::create("stone.png");
	stone1->setPosition(Vec2(560, 480));
	this->addChild(stone1, 1);
	auto moveTo1 = MoveTo::create(1.0f, location);
	auto fadeOut = FadeOut::create(0.8f);
	auto seq = Sequence::create(moveTo1, fadeOut, NULL);
	stone1->runAction(seq);

	Size visibleSize = Director::getInstance()->getVisibleSize();
	auto moveTo = MoveTo::create(1.0f, Vec2(random() % (int)visibleSize.width, random() % (int)visibleSize.height));
	mouse->runAction(moveTo);


	
}

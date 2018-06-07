#include "MenuScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* MenuScene::createScene()
{
    return MenuScene::create();
}

// Print useful error message instead of segfaulting when files are not there.
static void problemLoading(const char* filename)
{
    printf("Error while loading: %s\n", filename);
    printf("Depending on how you compiled you might have to add 'Resources/' in front of filenames in HelloWorldScene.cpp\n");
}

// on "init" you need to initialize your instance
bool MenuScene::init()
{
    //////////////////////////////
    // 1. super init first
    if ( !Scene::init() )
    {
        return false;
    }

    auto visibleSize = Director::getInstance()->getVisibleSize();
    Vec2 origin = Director::getInstance()->getVisibleOrigin();

	auto bg_sky = Sprite::create("menu-background-sky.jpg");
	bg_sky->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y + 150));
	this->addChild(bg_sky, 0);

	auto bg = Sprite::create("menu-background.png");
	bg->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y - 60));
	this->addChild(bg, 0);

	auto miner = Sprite::create("menu-miner.png");
	miner->setPosition(Vec2(150 + origin.x, visibleSize.height / 2 + origin.y - 60));
	this->addChild(miner, 1);

	auto leg = Sprite::createWithSpriteFrameName("miner-leg-0.png");
	Animate* legAnimate = Animate::create(AnimationCache::getInstance()->getAnimation("legAnimation"));
	leg->runAction(RepeatForever::create(legAnimate));
	leg->setPosition(110 + origin.x, origin.y + 102);
	this->addChild(leg, 1);

	auto title = Sprite::create("gold-miner-text.png");
	title->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height + origin.y - 150));
	this->addChild(title, 1);

	auto pic = Sprite::create("menu-start-gold.png");
	pic->setPosition(Vec2(visibleSize.width / 2 + origin.x + 300, origin.y + 150));
	this->addChild(pic, 1);

	auto start0 = Sprite::create("start-0.png");
	auto start1 = Sprite::create("start-1.png");
	auto item = MenuItemSprite::create(start0, start1, CC_CALLBACK_1(MenuScene::startMenuCallback, this));
	item->setPosition(Vec2(visibleSize.width / 2 + origin.x + 300, origin.y + 200));

	auto menu = Menu::create(item,NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, 1);

    return true;
}

void MenuScene::startMenuCallback(cocos2d::Ref * pSender)
{
	auto scene = GameSence::createScene();
	Director::getInstance()->replaceScene(TransitionFade::create(2, scene));
}


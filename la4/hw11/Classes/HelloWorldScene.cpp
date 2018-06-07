#include "HelloWorldScene.h"
#include "SimpleAudioEngine.h"
#pragma execution_character_set("utf-8")

USING_NS_CC;

Scene* HelloWorld::createScene()
{
    return HelloWorld::create();
}

// Print useful error message instead of segfaulting when files are not there.
static void problemLoading(const char* filename)
{
    printf("Error while loading: %s\n", filename);
    printf("Depending on how you compiled you might have to add 'Resources/' in front of filenames in HelloWorldScene.cpp\n");
}

// on "init" you need to initialize your instance
bool HelloWorld::init()
{
    //////////////////////////////
    // 1. super init first
    if ( !Scene::init() )
    {
        return false;
    }

    visibleSize = Director::getInstance()->getVisibleSize();
    origin = Director::getInstance()->getVisibleOrigin();
	en = false;

	//����һ����ͼ
	auto texture = Director::getInstance()->getTextureCache()->addImage("$lucia_2.png");
	//����ͼ�������ص�λ�и�����ؼ�֡
	auto frame0 = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(0, 0, 113, 113)));
	//ʹ�õ�һ֡��������
	player = Sprite::createWithSpriteFrame(frame0);
	player->setPosition(Vec2(origin.x + visibleSize.width / 2,
		origin.y + visibleSize.height / 2));
	addChild(player, 3);

	//hp��
	Sprite* sp0 = Sprite::create("hp.png", CC_RECT_PIXELS_TO_POINTS(Rect(0, 320, 420, 47)));
	Sprite* sp = Sprite::create("hp.png", CC_RECT_PIXELS_TO_POINTS(Rect(610, 362, 4, 16)));

	//ʹ��hp������progressBar
	pT = ProgressTimer::create(sp);
	pT->setScaleX(90);
	pT->setAnchorPoint(Vec2(0, 0));
	pT->setType(ProgressTimerType::BAR);
	pT->setBarChangeRate(Point(1, 0));
	pT->setMidpoint(Point(0, 1));
	pT->setPercentage(100);
	pT->setPosition(Vec2(origin.x + 14 * pT->getContentSize().width, origin.y + visibleSize.height - 2 * pT->getContentSize().height));
	addChild(pT, 1);
	sp0->setAnchorPoint(Vec2(0, 0));
	sp0->setPosition(Vec2(origin.x + pT->getContentSize().width, origin.y + visibleSize.height - sp0->getContentSize().height));
	addChild(sp0, 0);

	// ��̬����
	idle.reserve(1);
	idle.pushBack(frame0);

	// ��������
	attack.reserve(17);
	for (int i = 0; i < 17; i++) {
		auto frame = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(113 * i, 0, 113, 113)));
		attack.pushBack(frame);
	}

	// ���Է��չ�������
	// ��������(֡����22֡���ߣ�90������79��
	auto texture2 = Director::getInstance()->getTextureCache()->addImage("$lucia_dead.png");
	// Todo
	dead.reserve(22);
	for (int i = 0; i < 22; ++i) {
		auto frame= SpriteFrame::createWithTexture(texture2, CC_RECT_PIXELS_TO_POINTS(Rect(79 * i, 0, 79, 90)));
		dead.pushBack(frame);
	}

	// �˶�����(֡����8֡���ߣ�101������68��
	auto texture3 = Director::getInstance()->getTextureCache()->addImage("$lucia_forward.png");
	// Todo
	run.reserve(8);
	for (int i = 0; i < 8; ++i) {
		auto frame = SpriteFrame::createWithTexture(texture3, CC_RECT_PIXELS_TO_POINTS(Rect(68 * i, 0, 68, 101)));
		run.pushBack(frame);
	}

	auto w = Label::createWithTTF("W", "fonts/arial.ttf", 36);
	auto wItem = MenuItemLabel::create(w, CC_CALLBACK_1(HelloWorld::moveWCallback,this));
	wItem->setPosition(100, 100);

	auto s = Label::createWithTTF("S", "fonts/arial.ttf", 36);
	auto sItem = MenuItemLabel::create(s, CC_CALLBACK_1(HelloWorld::moveSCallback, this));
	sItem->setPosition(100, 50);

	auto a = Label::createWithTTF("A", "fonts/arial.ttf", 36);
	auto aItem = MenuItemLabel::create(a, CC_CALLBACK_1(HelloWorld::moveACallback, this));
	aItem->setPosition(50, 50);

	auto d = Label::createWithTTF("D", "fonts/arial.ttf", 36);
	auto dItem = MenuItemLabel::create(d, CC_CALLBACK_1(HelloWorld::moveDCallback, this));
	dItem->setPosition(150, 50);

	auto x = Label::createWithTTF("X", "fonts/arial.ttf", 36);
	auto xItem = MenuItemLabel::create(x, CC_CALLBACK_1(HelloWorld::moveXCallback, this));
	xItem->setPosition(visibleSize.width-50, 100);

	auto y = Label::createWithTTF("Y", "fonts/arial.ttf", 36);
	auto yItem = MenuItemLabel::create(y, CC_CALLBACK_1(HelloWorld::moveYCallback, this));
	yItem->setPosition(visibleSize.width-100, 50);

	auto menu = Menu::create(wItem, sItem, aItem, dItem, xItem, yItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, 1);

	dtime = 160;
	auto temp = CCString::createWithFormat("%d", dtime);
	time = Label::createWithTTF(temp->getCString(),"fonts/arial.ttf",36);
	time->setPosition(visibleSize.width / 2, visibleSize.height - 50);
	this->addChild(time,1);
	schedule(schedule_selector(HelloWorld::timeCallback), 1.0f, kRepeatForever, 1.0f);
    return true;
}

void HelloWorld::moveWCallback(Ref * pSender)
{
	MoveBy* d;
	auto c = player->getPosition();
	if (c.y + 20 > visibleSize.height - 30 )
		d = MoveBy::create(0.2f, Point(0, visibleSize.height -30 - c.y)); 
	else
		d = MoveBy::create(0.2f, Point(0, 20));
	auto animation = Animation::createWithSpriteFrames(run, 0.05f);
	auto animate = Animate::create(animation);
	auto spawn = Spawn::create(d, animate, NULL);
	player->runAction(spawn);
}

void HelloWorld::moveSCallback(Ref * pSender)
{
	MoveBy* d;
	auto c = player->getPosition();
	if (c.y - 20 < 30)
		d = MoveBy::create(0.2f, Point(30 - c.y, 0));
	else
		d = MoveBy::create(0.2f, Point(0, -20));
	auto animation = Animation::createWithSpriteFrames(run, 0.05f);
	auto animate = Animate::create(animation);
	auto spawn = Spawn::create(d, animate, NULL);
	player->runAction(spawn);
}

void HelloWorld::moveACallback(Ref * pSender)
{
	MoveBy* d;
	auto c = player->getPosition();
	if (c.x - 20 < 30)
		d = MoveBy::create(0.2f, Point(30-c.x, 0));
	else
		d = MoveBy::create(0.2f, Point(-20, 0));
	auto animation = Animation::createWithSpriteFrames(run, 0.05f);
	auto animate = Animate::create(animation);
	auto spawn = Spawn::create(d, animate, NULL);
	player->runAction(spawn);
}

void HelloWorld::moveDCallback(Ref * pSender)
{
	MoveBy* d;
	auto c = player->getPosition();
	if (c.x + 20 > visibleSize.width - 30)
		d = MoveBy::create(0.2f, Point(visibleSize.width-30 - c.x, 0));
	else
		d = MoveBy::create(0.2f, Point(20, 0));
	auto animation = Animation::createWithSpriteFrames(run, 0.05f);
	auto animate = Animate::create(animation);
	auto spawn = Spawn::create(d, animate, NULL);
	player->runAction(spawn);
}

void HelloWorld::moveXCallback(Ref * pSender)
{
	auto animation = Animation::createWithSpriteFrames(dead, 0.1f);
	auto animate = Animate::create(animation);
	auto idleAnimation = Animation::createWithSpriteFrames(idle, 0.1f);
	auto idleAnimate = Animate::create(idleAnimation);
	auto c = pT->getPercentage();
	c -= 20;
	if (c < 0)
		c = 0;
	CCProgressTo* ac1 = CCProgressTo::create(2.0f, c);
	auto seq = Sequence::create(animate, idleAnimate, CCCallFunc::create(this, callfunc_selector(HelloWorld::enCallback)), NULL);
	if (en == false) {
		en = true;
		player->runAction(seq);
		pT->runAction(ac1);
	}

}

void HelloWorld::moveYCallback(Ref * pSender)
{
	auto animation = Animation::createWithSpriteFrames(attack, 0.1f);
	auto animate = Animate::create(animation);
	auto idleAnimation = Animation::createWithSpriteFrames(idle, 0.1f);
	auto idleAnimate = Animate::create(idleAnimation);
	auto c = pT->getPercentage();
	c += 20;
	if (c > 100)
		c = 100;
	CCProgressTo* ac1 = CCProgressTo::create(2.0f, c);
	auto seq = Sequence::create(animate, idleAnimate, CCCallFunc::create(this, callfunc_selector(HelloWorld::enCallback)), NULL);
	if (en == false) {
		en = true;
		player->runAction(seq);
		pT->runAction(ac1);
	}
}

void HelloWorld::enCallback()
{
	en = false;
}

void HelloWorld::timeCallback(float dt)
{
	dtime--;
	auto temp = CCString::createWithFormat("%d", dtime);
	time->setString(temp->getCString());
}


#pragma execution_character_set("utf-8")
#include "HitBrick.h"
#include "cocos2d.h"
#include "SimpleAudioEngine.h"
#define database UserDefault::getInstance()

USING_NS_CC;
using namespace CocosDenshion;

void HitBrick::setPhysicsWorld(PhysicsWorld* world) { m_world = world; }

Scene* HitBrick::createScene() {
	srand((unsigned)time(NULL));
	auto scene = Scene::createWithPhysics();

	scene->getPhysicsWorld()->setAutoStep(true);

	// Debug ģʽ
	//scene->getPhysicsWorld()->setDebugDrawMask(PhysicsWorld::DEBUGDRAW_ALL);
	scene->getPhysicsWorld()->setGravity(Vec2(0, -100));
	auto layer = HitBrick::create();
	layer->setPhysicsWorld(scene->getPhysicsWorld());
	layer->setJoint();
	scene->addChild(layer);
	return scene;
}

// on "init" you need to initialize your instance
bool HitBrick::init() {
	//////////////////////////////
	// 1. super init first
	if (!Layer::init()) {
		return false;
	}
	visibleSize = Director::getInstance()->getVisibleSize();


	auto edgeSp = Sprite::create();  //����һ������
	auto boundBody = PhysicsBody::createEdgeBox(visibleSize, PhysicsMaterial(0.0f, 1.0f, 0.0f), 3);  //edgebox�ǲ��ܸ�����ײӰ���һ�ָ��壬����������������������ı߽�
	edgeSp->setPosition(visibleSize.width / 2, visibleSize.height / 2);  //λ����������Ļ����
	edgeSp->setPhysicsBody(boundBody);
	addChild(edgeSp);


	preloadMusic(); // Ԥ������Ч

	addSprite();    // ��ӱ����͸��־���
	addListener();  // ��Ӽ����� 
	addPlayer();    // ��������
	BrickGeneraetd();  // ����ש��


	schedule(schedule_selector(HitBrick::updateShip), 0.01f, kRepeatForever, 0.1f);

	onBall = true;
	spHolded = true;
	spFactor = 0;
	
	return true;
}

// �ؽ����ӣ��̶��������
// Todo
void HitBrick::setJoint() {
	joint1 = PhysicsJointPin::construct(
		player->getPhysicsBody(), ball->getPhysicsBody(), Vec2(1,30),Vec2(0,0));
	joint1->setCollisionEnable(true);
	m_world->addJoint(joint1);
}

// Ԥ������Ч
void HitBrick::preloadMusic() {
	auto sae = SimpleAudioEngine::getInstance();
	sae->preloadEffect("gameover.mp3");
	sae->preloadBackgroundMusic("bgm.mp3");
	sae->playBackgroundMusic("bgm.mp3", true);
}

// ��ӱ����͸��־���
void HitBrick::addSprite() {
	// add background
	auto bgSprite = Sprite::create("bg.png");
	bgSprite->setPosition(visibleSize / 2);
	bgSprite->setScale(visibleSize.width / bgSprite->getContentSize().width, visibleSize.height / bgSprite->getContentSize().height);
	this->addChild(bgSprite, 0);


	// add ship
	ship = Sprite::create("ship.png");
	ship->setScale(visibleSize.width / ship->getContentSize().width * 0.97, 1.2f);
	ship->setPosition(visibleSize.width / 2, 0);
	auto shipbody = PhysicsBody::createBox(ship->getContentSize(), PhysicsMaterial(100.0f, 0.0f, 1.0f));
	shipbody->setCategoryBitmask(0xFFFFFFFF);
	shipbody->setCollisionBitmask(0xFFFFFFFF);
	shipbody->setContactTestBitmask(0xFFFFFFFF);
	ship->setTag(1);
	shipbody->setDynamic(false);  // ??????�I?????????, ????????????��??
	ship->setPhysicsBody(shipbody);
	this->addChild(ship, 1);

	// add sun and cloud
	auto sunSprite = Sprite::create("sun.png");
	sunSprite->setPosition(rand() % (int)(visibleSize.width - 200) + 100, 550);
	this->addChild(sunSprite);
	auto cloudSprite1 = Sprite::create("cloud.png");
	cloudSprite1->setPosition(rand() % (int)(visibleSize.width - 200) + 100, rand() % 100 + 450);
	this->addChild(cloudSprite1);
	auto cloudSprite2 = Sprite::create("cloud.png");
	cloudSprite2->setPosition(rand() % (int)(visibleSize.width - 200) + 100, rand() % 100 + 450);
	this->addChild(cloudSprite2);
}

// ��Ӽ�����
void HitBrick::addListener() {
	auto keyboardListener = EventListenerKeyboard::create();
	keyboardListener->onKeyPressed = CC_CALLBACK_2(HitBrick::onKeyPressed, this);
	keyboardListener->onKeyReleased = CC_CALLBACK_2(HitBrick::onKeyReleased, this);
	_eventDispatcher->addEventListenerWithSceneGraphPriority(keyboardListener, this);

	auto contactListener = EventListenerPhysicsContact::create();
	contactListener->onContactBegin = CC_CALLBACK_1(HitBrick::onConcactBegin, this);
	_eventDispatcher->addEventListenerWithSceneGraphPriority(contactListener, this);
}

// ������ɫ
void HitBrick::addPlayer() {

	player = Sprite::create("bar.png");
	int xpos = visibleSize.width / 2;

	player->setScale(0.1f, 0.1f);
	player->setPosition(Vec2(xpos, ship->getContentSize().height - player->getContentSize().height*0.1f));
	// ���ð�ĸ�������
	// Todo
	auto physicsBody = PhysicsBody::createBox(player->getContentSize());
	physicsBody->getShape(0)->setRestitution(1);
	physicsBody->setDynamic(false);
	physicsBody->setGravityEnable(false);
	physicsBody->setCategoryBitmask(0xffffffff);
	physicsBody->setCollisionBitmask(0xffffffff);
	physicsBody->setContactTestBitmask(0xffffffff);
	player->setPhysicsBody(physicsBody);

	this->addChild(player, 2);

	ball = Sprite::create("ball.png");
	ball->setPosition(Vec2(xpos, player->getPosition().y + ball->getContentSize().height*0.1f));
	ball->setScale(0.1f, 0.1f);
	// ������ĸ�������
	// Todo
	physicsBody = PhysicsBody::createCircle(ball->getContentSize().width/2);
	physicsBody->setMass(100);
	physicsBody->getShape(0)->setRestitution(1);
	physicsBody->setDynamic(true);
	physicsBody->setGravityEnable(false);
	physicsBody->setVelocity(Vec2(0, 200));
	physicsBody->setCategoryBitmask(0xffffffff);
	physicsBody->setCollisionBitmask(0xffffffff);
	physicsBody->setContactTestBitmask(0xffffffff);
	ball->setPhysicsBody(physicsBody);

	addChild(ball, 3);

	ParticleMeteor* fireworks = ParticleMeteor::create();
	ball->addChild(fireworks);

}

// ʵ�ּ򵥵�����Ч��
// Todo
void HitBrick::update(float dt) {
	spFactor += 500;
}

void HitBrick::updateShip(float dt)
{
	
	if (player->getPosition().x<50 || player->getPosition().x>visibleSize.width-50) {
		if (spHolded) {
			player->getPhysicsBody()->setVelocity(Vec2(0, 0));
			spHolded = false;
		}
	}
	else {
		spHolded = true;
	}
	
}

// Todo
void HitBrick::BrickGeneraetd() {
	visibleSize = Director::getInstance()->getVisibleSize();
	for (int i = 0; i < 3; i++) {
		int cw = 0;
		while (cw <= visibleSize.width) {
			auto box = Sprite::create("box.png");
			// Ϊש�����ø�������
			// Todo
			auto physicsBody = PhysicsBody::createBox(box->getContentSize());
			physicsBody->getShape(0)->setRestitution(1);
			physicsBody->setDynamic(false);
			physicsBody->setGravityEnable(false);
			physicsBody->setCategoryBitmask(0xffffffff);
			physicsBody->setCollisionBitmask(0xffffffff);
			physicsBody->setContactTestBitmask(0xffffffff);

			box->setPhysicsBody(physicsBody);
			box->setTag(10);
			box->setPosition(cw, visibleSize.height - (i+1) * (box->getContentSize().height+1));
			this->addChild(box,2);
			cw += box->getContentSize().width+2;
		}

	}

}


// ����
void HitBrick::onKeyPressed(EventKeyboard::KeyCode code, Event* event) {

	switch (code) {
	case cocos2d::EventKeyboard::KeyCode::KEY_LEFT_ARROW:
		if (player->getPosition().x > 50)
			player->getPhysicsBody()->setVelocity(Vec2(-400, 0));

		break;
	case cocos2d::EventKeyboard::KeyCode::KEY_RIGHT_ARROW:
		// �����ƶ�
		// Todo
		if (player->getPosition().x < visibleSize.width - 50)
			player->getPhysicsBody()->setVelocity(Vec2(400, 0));
		break;

	case cocos2d::EventKeyboard::KeyCode::KEY_SPACE: // ��ʼ����
		this->scheduleUpdate();
		spFactor = 0;
		break;
	default:
		break;
	}
}

// �ͷŰ���
void HitBrick::onKeyReleased(EventKeyboard::KeyCode code, Event* event) {
	switch (code) {
	case cocos2d::EventKeyboard::KeyCode::KEY_LEFT_ARROW:
	case cocos2d::EventKeyboard::KeyCode::KEY_RIGHT_ARROW:
		// ֹͣ�˶�
		// Todo
		player->getPhysicsBody()->setVelocity(Vec2(0, 0));
		break;
	case cocos2d::EventKeyboard::KeyCode::KEY_SPACE:   // ����������С����
		m_world->removeAllJoints();
		this->unscheduleUpdate();
		ball->getPhysicsBody()->applyImpulse(Vec2(0, spFactor));
		CCLOG("%d", spFactor);
		break;

	default:
		break;
	}
}

// ��ײ���
// Todo
bool HitBrick::onConcactBegin(PhysicsContact & contact) {
	auto c1 = contact.getShapeA()->getBody()->getNode();
	auto c2 = contact.getShapeB()->getBody()->getNode();
	
	if (c1&&c2) {
		if (c1->getTag() == 1 || c2->getTag() == 1) {
			GameOver();
		}
		else if (c1->getTag() == 10) {
			c1->removeFromParentAndCleanup(true);
		}
		else if (c2->getTag() == 10) {
			c2->removeFromParentAndCleanup(true);
		}
	}
	return true;
}


void HitBrick::GameOver() {

	_eventDispatcher->removeAllEventListeners();
	ball->getPhysicsBody()->setVelocity(Vec2(0, 0));
	player->getPhysicsBody()->setVelocity(Vec2(0, 0));
	ball->getPhysicsBody()->setAngularVelocity(0);
	SimpleAudioEngine::getInstance()->stopBackgroundMusic("bgm.mp3");
	SimpleAudioEngine::getInstance()->playEffect("gameover.mp3", false);

	auto label1 = Label::createWithTTF("Game Over~", "fonts/STXINWEI.TTF", 60);
	label1->setColor(Color3B(0, 0, 0));
	label1->setPosition(visibleSize.width / 2, visibleSize.height / 2);
	this->addChild(label1);

	auto label2 = Label::createWithTTF("����", "fonts/STXINWEI.TTF", 40);
	label2->setColor(Color3B(0, 0, 0));
	auto replayBtn = MenuItemLabel::create(label2, CC_CALLBACK_1(HitBrick::replayCallback, this));
	Menu* replay = Menu::create(replayBtn, NULL);
	replay->setPosition(visibleSize.width / 2 - 80, visibleSize.height / 2 - 100);
	this->addChild(replay);

	auto label3 = Label::createWithTTF("�˳�", "fonts/STXINWEI.TTF", 40);
	label3->setColor(Color3B(0, 0, 0));
	auto exitBtn = MenuItemLabel::create(label3, CC_CALLBACK_1(HitBrick::exitCallback, this));
	Menu* exit = Menu::create(exitBtn, NULL);
	exit->setPosition(visibleSize.width / 2 + 90, visibleSize.height / 2 - 100);
	this->addChild(exit);
}

// ���������水ť��Ӧ����
void HitBrick::replayCallback(Ref * pSender) {
	Director::getInstance()->replaceScene(HitBrick::createScene());
}

// �˳�
void HitBrick::exitCallback(Ref * pSender) {
	Director::getInstance()->end();
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
	exit(0);
#endif
}

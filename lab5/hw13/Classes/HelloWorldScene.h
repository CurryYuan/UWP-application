#pragma once
#include "cocos2d.h"
#include "Monster.h"
using namespace cocos2d;

class HelloWorld : public cocos2d::Scene
{
public:
    static cocos2d::Scene* createScene();

    virtual bool init();
	virtual void moveWCallback(Ref* pSender);
	virtual void moveSCallback(Ref* pSender);
	virtual void moveACallback(Ref* pSender);
	virtual void moveDCallback(Ref* pSender);
	virtual void moveXCallback(Ref* pSender);
	virtual void moveYCallback(Ref* pSender);
	virtual void enCallback();
	virtual void endCallback();
	virtual void pauseCallback();
	virtual void timeCallback(float dt);
	virtual void moveCallback(float dt);
	virtual void hitByMonster(float dt);
    // implement the "static create()" method manually
    CREATE_FUNC(HelloWorld);
private:
	cocos2d::Sprite* player;
	cocos2d::Vector<SpriteFrame*> attack;
	cocos2d::Vector<SpriteFrame*> dead;
	cocos2d::Vector<SpriteFrame*> run;
	cocos2d::Vector<SpriteFrame*> idle;
	cocos2d::Size visibleSize;
	cocos2d::Vec2 origin;
	cocos2d::Label* time;
	cocos2d::Label* score;
	int dtime, sc,hp;
	bool en;
	cocos2d::ProgressTimer* pT;


};

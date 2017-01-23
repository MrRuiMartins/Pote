# Pote
Pote is a virtual catch trial game.

Backend functionality is written in .net core.

Frontend functionality is written in ReactJS.

Use docker to tie it all together.

-------------------------
Gameplay
------------------------

There are three types of roles in Pote: Catchers and Hunters, and Mediators.

The goal of Catchers is to find Hunters.

The goal of Hunters is to collect tokens.

When a hunter finds a token location, a challenge appears that if successfuly solved will be redeemed in tokens.

Mediators are external actors that can add challenges (and token locations) to the system.
These challenges are human only challenges.

Upon catching a Hunter, the Catcher has the option to include a transaction in the next blockchain block.

Upon completion of a challenge the outcome is reported in a trust chain via blockchain technology.

The challenge can be donating to distributed computing projects.

-------------------------

Motivation for each actor
---------------

* Mediators want some computation performed.
* Hunters want tokens in order to have a high ranking in the blockchain contributor list.
* Catchers want to include a transaction in the blockchain.

----------------------


mediators issue a challenge.
the challenge consists of a description and a hash of the solution.
hunters get the challenge and try to solve it.
upon solving, the challenger and the solver are linked together

---------------------

Service that lets you add cryptographic intellectual property in block chain
-------------------------

# API

Mediator
----------------
Entity:
* Id

Actions:
* Insert challenge

-----------------
Hunter
--------------------
Entity:
* Id

Actions:
* Solve Challenge

----------------------------------
Challenge
----------------------
Entity:
* Hash Solution
* Challenge Description
* Reward (points for now)
* MediatorId

Actions:
* Get Challenges

-----------------
About
----------------

Replies with information on the project.

# Pote
Pote is a virtual catch trial game.

Backend functionality is written in .NET Core.

Frontend functionality is written in ReactJS.

Use docker to tie it all together.

-------------------------
Gameplay
------------------------

There are two types of roles in Pote: Hunters, and Mediators.

The goal of Hunters is to collect tokens.

When a hunter finds a token location, a challenge appears that if successfuly solved will be redeemed in tokens.

Mediators are external actors that can add challenges (and token locations) to the system.

Upon completion of a challenge the outcome is reported in a trust chain via blockchain technology.

-------------------------

Motivation for each actor
---------------
* Mediators want some computation performed.
* Hunters want tokens in order to have a high ranking in the blockchain contributor list.
----------------------
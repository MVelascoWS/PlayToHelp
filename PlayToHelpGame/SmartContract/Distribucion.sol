/// SPDX-License-Identifier: MIT
pragma solidity ^0.8.14;

import {ISuperfluid, ISuperToken, SuperAppBase, SuperAppDefinitions} from "@superfluid-finance/ethereum-contracts/contracts/apps/SuperAppBase.sol";
import {IInstantDistributionAgreementV1} from "@superfluid-finance/ethereum-contracts/contracts/interfaces/agreements/IInstantDistributionAgreementV1.sol";

import {IDAv1Library} from "@superfluid-finance/ethereum-contracts/contracts/apps/IDAv1Library.sol";

import {IERC20} from "@openzeppelin/contracts/token/ERC20/ERC20.sol";

contract TokenSpreader {
    
    ISuperToken public spreaderToken;

    using IDAv1Library for IDAv1Library.InitData;
    IDAv1Library.InitData public idaV1;

    uint32 public constant INDEX_ID = 0;

    constructor(ISuperfluid _host, ISuperToken _spreaderToken) {
        spreaderToken = _spreaderToken;

        idaV1 = IDAv1Library.InitData(
            _host,
            IInstantDistributionAgreementV1(
                address(
                    _host.getAgreementClass(
                        keccak256(
                            "org.superfluid-finance.agreements.InstantDistributionAgreement.v1"
                        )
                    )
                )
            )
        );

        idaV1.createIndex(_spreaderToken, INDEX_ID);
    }


    function distribute() public {
        uint256 spreaderTokenBalance = spreaderToken.balanceOf(address(this));

        (uint256 actualDistributionAmount, ) = idaV1.ida.calculateDistribution(
            spreaderToken,
            address(this),
            INDEX_ID,
            spreaderTokenBalance
        );

        idaV1.distribute(spreaderToken, INDEX_ID, actualDistributionAmount);
    }

    function gainShare(address subscriber) public {

        (, , uint256 currentUnitsHeld, ) = idaV1.getSubscription(
            spreaderToken,
            address(this),
            INDEX_ID,
            subscriber
        );

        idaV1.updateSubscriptionUnits(
            spreaderToken,
            INDEX_ID,
            subscriber,
            uint128(currentUnitsHeld + 1)
        );
    }


    function loseShare(address subscriber) public {

        (, , uint256 currentUnitsHeld, ) = idaV1.getSubscription(
            spreaderToken,
            address(this),
            INDEX_ID,
            subscriber
        );

        idaV1.updateSubscriptionUnits(
            spreaderToken,
            INDEX_ID,
            subscriber,
            uint128(currentUnitsHeld - 1)
        );
    }

    function deleteShares(address subscriber) public {
        idaV1.deleteSubscription(spreaderToken, address(this), INDEX_ID, subscriber);
    }
}
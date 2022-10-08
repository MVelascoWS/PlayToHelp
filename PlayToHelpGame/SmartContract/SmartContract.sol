// SPDX-License-Identifier: AGPL-3.0-only
pragma solidity ^0.8.14;

import "https://github.com/UMAprotocol/protocol/blob/master/packages/core/contracts/oracle/interfaces/OptimisticOracleV2Interface.sol";

contract OO_GettingStarted {
    
    OptimisticOracleV2Interface oo = OptimisticOracleV2Interface(0xAB75727d4e89A7f7F04f57C00234a35950527115);

    bytes32 identifier = bytes32("YES_OR_NO_QUERY");

    bytes ancillaryData =
        bytes("scholarship approval? A:1 for yes. 0 for no.");

    uint256 requestTime = 0; 

    function requestData() public {
        requestTime = block.timestamp; 
        IERC20 bondCurrency = IERC20(0x910c98B3EAc2B4c3f6FdB81882bfd0161e507567); 
        uint256 reward = 0; 

            oo.requestPrice(identifier, requestTime, ancillaryData, bondCurrency, reward);
        oo.setCustomLiveness(identifier, requestTime, ancillaryData, 19);
    }

    function settleRequest() public {
        oo.settle(address(this), identifier, requestTime, ancillaryData);
    }

    function getSettledData() public view returns (int256) {
        return oo.getRequest(address(this), identifier, requestTime, ancillaryData).resolvedPrice;
    }
}
// SPDX-License-Identifier: AGPLv3
pragma solidity ^0.8.0;

import {SuperTokenBase} from "https://github.com/superfluid-finance/custom-supertokens/blob/main/contracts/base/SuperTokenBase.sol";

/// @title Minimal Pure Super Token
/// @author jtriley.eth
/// @notice Pre-minted supply. This is includes no custom logic. Used in `PureSuperTokenDeployer`
contract PureSuperToken is SuperTokenBase {

	/// @dev Upgrades the super token with the factory, then initializes.
    function initialize(
    ) external {
        _initialize(0x200657E2f123761662567A1744f9ACAe50dF47E6, "zet", "ZET");
        _mint(0x4b7ee35c2386893d9984a701E2f98D91b1Ed3DfA, 1000000000000000000000000, "");
    }

}
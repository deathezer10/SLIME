﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorable  {

    void OnStore(BeltSlot slot);

    void OnUnStore(BeltSlot slot);

}

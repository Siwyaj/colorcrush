mergeInto(LibraryManager.library, {

  JS_CreateUser: function(userIdPtr) {
    var userId = UTF8ToString(userIdPtr);
    const userRef = db.collection("users").doc(userId);
    userRef.set({
      userId: userId,
      createdAt: new Date().toISOString()
    }).then(() => console.log("User created: " + userId))
      .catch(err => console.error(err));
  },

  JS_WriteDemographicData: function(userIdPtr, jsonDataPtr) {
    var userId = UTF8ToString(userIdPtr);
    var jsonData = UTF8ToString(jsonDataPtr);
    const data = JSON.parse(jsonData);
    const userRef = db.collection("users").doc(userId);
    userRef.collection("demographics").add(data)
      .then(() => console.log("Demographics added for " + userId))
      .catch(err => console.error(err));
  },

  JS_AppendColorLog: function(userIdPtr, colorNamePtr, logDataPtr) {
    var userId = UTF8ToString(userIdPtr);
    var colorName = UTF8ToString(colorNamePtr);
    var logData = UTF8ToString(logDataPtr);
    const colorRef = db.collection("users").doc(userId)
      .collection("colors").doc(colorName);
    colorRef.set({
      logs: firebase.firestore.FieldValue.arrayUnion(logData),
      timestamp: new Date().toISOString()
    }, { merge: true });
  }

});

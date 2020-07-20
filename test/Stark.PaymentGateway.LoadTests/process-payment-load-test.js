import { sleep, check } from "k6";
import http from "k6/http";

import { authenticate } from "./Auth.js";

export const options = {
    stages: [
        { duration: "10s", target: 1 },
        { duration: "45s", target: 1 },
        { duration: "5s", target: 0 },
    ],
    thresholds: {
        "http_req_duration": ["p(95)<750"]
    }
};

let url = "https://localhost:5001/api/v1.0/payment";

let generateBody = function () {
    return {
        "id": generateGUID(),
        "amount": 9.99,
        "currency": "GBP",
        "cardNumber": "4242424242424242",
        "expMonth": 1,
        "expYear": 99,
        "cvv": "123"
    }
};

let generateGUID = function () {
    //this uses an answer from Stack Overflow - https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
};

export default function () {
    let headers = {
        "Authorization": `Bearer ${authenticate()}`,
        "content-type": "application/json"
    };
    let body = JSON.stringify(generateBody());
    let response = http.post(url, body, { headers });

    check(response, {
        "The response was '200 OK'": response => response.status.toString() === "200"
    });

    sleep(0.5);
};
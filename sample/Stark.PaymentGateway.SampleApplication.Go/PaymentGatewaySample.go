package main

import PaymentGatewaySDK "../../src/Stark.PaymentGateway.Sdk.Go"
import "fmt"

func main() {
	client := PaymentGatewaySDK.NewClient("insert a valid token")
	details, err := client.Retrieve("insert id of existing payment")

	if err != nil {
		fmt.Printf(err.Error())
	} else {
		fmt.Printf("details retrieved for ")
		fmt.Printf(details.Id)		
	}
}

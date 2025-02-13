# RainfallApi

Exposes an API as defined below, sourcing data from data.gov.uk

```
openapi: 3.1.0
info:
  title: Rainfall Api
  version: '1.0'
  contact:
    name: Sorted
    url: 'https://www.sorted.com'
  description: An API which provides rainfall reading data
servers:
  - url: 'http://localhost:3000'
    description: Rainfall Api
tags:
  - name: Rainfall
    description: Operations relating to rainfall
paths:
  '/rainfall/id/{stationId}/readings':
    parameters:
      - schema:
          type: string
        name: stationId
        in: path
        required: true
        description: The id of the reading station
      - schema:
          type: number
          minimum: 1
          maximum: 100
        name: count
        in: query
        required: false
        default: 10
        description: The number of readings to return
    get:
      operationId: get-rainfall
      summary: Get rainfall readings by station Id
      description: Retrieve the latest readings for the specified stationId
      tags:
        - Rainfall
      responses:
        '200':
          description: A list of rainfall readings successfully retrieved
          content:
            application/json:
              schema:
                $ref: '#/components/responses/rainfallReadingResponse'
        '400':
          description: Invalid request
          content:
            application/json:
              schema:
                $ref: '#/components/responses/errorResponse'
        '404':
          description: No readings found for the specified stationId
          content:
            application/json:
              schema:
                $ref: '#/components/responses/errorResponse'
        '500':
          description: Internal server error
          content:
            application/json:
              schema:
                $ref: '#/components/responses/errorResponse'
components:
  schemas:
    rainfallReadingResponse:
      title: Rainfall reading response
      type: object
      description: Details of a rainfall reading
      properties:
        readings:
          type: array
          items:
            $ref: '#/components/schemas/rainfallReading'
    rainfallReading:
      title: Rainfall reading
      type: object
      description: Details of a rainfall reading
      properties:
        dateMeasured:
          type: string
          format: date-time
        amountMeasured:
          type: number
          format: decimal
    error:
      title: Error response
      type: object
      description: Details of a rainfall reading
      properties:
        message:
          type: string
        detail:
          type: array
          items:
            type: array
            $ref: '#/components/schemas/errorDetail'
        additionalProperties: false
    errorDetail:
      type: object
      description: Details of invalid request property
      properties:
        propertyName:
          type: string
        message:
          type: string
        additionalProperties: false
  responses:
    rainfallReadingResponse:
      description: Get rainfall readings response
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/rainfallReadingResponse'
    errorResponse:
      description: An error object returned for failed requests
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/error'
```